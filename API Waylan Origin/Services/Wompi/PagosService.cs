using API_Waylan_Origin.Data;
using API_Waylan_Origin.DTOs.Wompi;
using API_Waylan_Origin.Interfaces.Wompi;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace API_Waylan_Origin.Services.Wompi
{
    public class PagosService : IPagosService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public PagosService(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task ProcesarNotificacionAsync(WompiWebHookDto notificacion)
        {
            // 1. Ignorar si no es una actualización de transacción
            if (notificacion.Event != "transaction.updated") return;

            var transaccion = notificacion.Data.Transaction;

            // 2. VALIDACIÓN DE SEGURIDAD (ANTI-FRAUDE)
            string secret = _config["WompiSettings:EventsSecret"];

            // Wompi pide concatenar: id + status + amount_in_cents + timestamp + secret
            string cadenaAVerificar = $"{transaccion.Id}{transaccion.Status}{transaccion.AmountInCents}{notificacion.Timestamp}{secret}";

            Console.WriteLine($"\n--- DEBUG WOMPI ---");
            Console.WriteLine($"ID: {transaccion.Id}");
            Console.WriteLine($"STATUS: {transaccion.Status}");
            Console.WriteLine($"MONTO: {transaccion.AmountInCents}");
            Console.WriteLine($"TIMESTAMP: {notificacion.Timestamp}");
            Console.WriteLine($"SECRET: {secret.Substring(0, 4)}... (Oculto por seguridad)");
            Console.WriteLine($"CADENA A HASHEAR: {cadenaAVerificar}");
            Console.WriteLine($"-------------------\n");

            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(cadenaAVerificar));
                string hashCalculado = BitConverter.ToString(bytes).Replace("-", "").ToLower();

                // Si el hash que calculamos no es igual al que envió Wompi, es un fraude.
                if (hashCalculado != notificacion.Signature.Checksum.ToLower())
                {
                    Console.WriteLine("⚠️ ALERTA DE SEGURIDAD: Firma de Wompi inválida.");
                    return; // Cortamos la ejecución, no actualizamos la BD.
                }
            }

            // 3. ACTUALIZAR LA BASE DE DATOS
          
            string codigo = transaccion.Reference;

                var pedido = await _context.pedidos.FirstOrDefaultAsync(p => p.CodigoSeguimiento == codigo);

                if (pedido != null)
                {
                    // Actualizamos el estado según lo que diga Wompi
                    if (transaccion.Status == "APPROVED")
                    {
                        pedido.EstadoPago = "Aprobado";
                    }
                    else if (transaccion.Status == "DECLINED" || transaccion.Status == "ERROR")
                    {
                        pedido.EstadoPago = "Rechazado";
                    }

                    // Opcional: Guardamos el ID de la transacción de Wompi para soporte técnico
                    pedido.WompiTransactionId = transaccion.Id;
                    pedido.FechaPago = DateTime.UtcNow;

                    await _context.SaveChangesAsync();
                    Console.WriteLine($"✅ Pedido {codigo} actualizado a {pedido.EstadoPago}");
                }
        }
    }
}
