public class OrderConfirmationModel
{
    // Allmänna egenskaper
    public string ConnectionString { get; set; } = string.Empty; // För e-postklienten
    public string Receiver { get; set; } = string.Empty; // Mottagarens e-postadress
    public string ExtraContent { get; set; } = string.Empty; // Extrainnehåll, t.ex. kundens namn

    // Betalningssession
    public string CustomerEmail { get; set; } = string.Empty; // Kundens e-post
    public string CustomerName { get; set; } = string.Empty; // Kundens namn
    public string OrderId { get; set; } = string.Empty; // Ordernummer
    public string PaymentMethodType { get; set; } = string.Empty; // Betalningsmetod
    public string PaymentMethodInfo { get; set; } = string.Empty;
    public string Currency { get; set; } = "USD"; // Valuta

    // Belopp och datum
    public long AmountSubtotal { get; set; } // Subtotalbelopp
    public long AmountTotal { get; set; } // Totalt belopp
    public DateTime PaymentDate { get; set; } // Datum för betalning

    // Frakt och skatt
    public long ShippingCost { get; set; } // Fraktkostnad
    public long Tax { get; set; } // Skattebelopp

    // Adress
    public string AddressLine1 { get; set; } = string.Empty; // Gatuadress
    public string PostalCode { get; set; } = string.Empty; // Postnummer
    public string City { get; set; } = string.Empty; // Stad
    public string Country { get; set; } = string.Empty; // Land
    public string PhoneNumber { get; set; } = string.Empty; // Telefon

    // Produkter
    public List<ProductModel> LineItems { get; set; } = new(); // Lista med produkter

    public class ProductModel
    {
        public string Description { get; set; } = string.Empty; // Produktbeskrivning
        public int Quantity { get; set; } // Antal
        public long UnitAmount { get; set; } // Pris per enhet
    }
}
