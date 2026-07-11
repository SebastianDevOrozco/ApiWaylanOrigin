using System.Text.Json.Serialization;

namespace API_Waylan_Origin.DTOs.Wompi
{
    public class WompiWebHookDto
    {
        [JsonPropertyName("event")]
        public string Event { get; set; } // Nos dirá si es "transaction.updated"

        [JsonPropertyName("data")]
        public WompiData Data { get; set; }

        [JsonPropertyName("Signature")]
        public WompiSignature Signature { get; set; }

        [JsonPropertyName("timestamp")]
        public long Timestamp { get; set; }
    }
    public class WompiData
    {

        [JsonPropertyName("transaction")]
        public WompiTransaction Transaction { get; set; }
    }

    public class WompiTransaction
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } // El ID único de Wompi

        [JsonPropertyName("status")]
        public string Status { get; set; } // APPROVED, DECLINED, VOIDED, ERROR

        [JsonPropertyName("reference")]
        public string Reference { get; set; } // Este será el ID de tu Pedido (ej. "105")

        [JsonPropertyName("amount_in_cents")]
        public int AmountInCents { get; set; } // Wompi maneja todo en centavos
    }

    public class WompiSignature
    {
        [JsonPropertyName("checksum")]
        public string Checksum { get; set; } // El código de seguridad para validar
    }
}
