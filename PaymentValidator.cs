using System.Linq;

namespace KGHCashierPOS
{
    public static class PaymentValidator
    {
        public static ValidationResult ValidateCashPayment(string cashText, decimal requiredAmount)
        {
            if (string.IsNullOrWhiteSpace(cashText))
            {
                return new ValidationResult
                {
                    IsValid = false,
                    Message = "Please enter cash amount",
                    DisplayText = "₱0.00"
                };
            }

            if (!decimal.TryParse(cashText, out decimal cashReceived))
            {
                return new ValidationResult
                {
                    IsValid = false,
                    Message = "Invalid amount",
                    DisplayText = "Invalid amount"
                };
            }

            if (cashReceived < requiredAmount)
            {
                return new ValidationResult
                {
                    IsValid = false,
                    Message = "Insufficient",
                    DisplayText = "Insufficient",
                    CashReceived = cashReceived
                };
            }

            decimal change = cashReceived - requiredAmount;
            return new ValidationResult
            {
                IsValid = true,
                Message = "Valid",
                DisplayText = PriceFormatter.Format(change),
                CashReceived = cashReceived,
                Change = change
            };
        }

        public static ValidationResult ValidateGCashReference(string reference)
        {
            if (string.IsNullOrWhiteSpace(reference))
            {
                return new ValidationResult
                {
                    IsValid = false,
                    Message = "Please enter GCash reference"
                };
            }

            // Remove spaces and dashes
            string cleanRef = reference.Replace(" ", "").Replace("-", "");

            // Check if all digits
            if (!cleanRef.All(char.IsDigit))
            {
                return new ValidationResult
                {
                    IsValid = false,
                    Message = "Reference must contain only numbers"
                };
            }

            // Valid length: 12-14 digits
            if (cleanRef.Length >= 12 && cleanRef.Length <= 14)
            {
                return new ValidationResult
                {
                    IsValid = true,
                    Message = "Valid",
                    GCashReference = cleanRef
                };
            }

            return new ValidationResult
            {
                IsValid = false,
                Message = "Reference must be 12-14 digits"
            };
        }
    }

    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public string Message { get; set; }
        public string DisplayText { get; set; }
        public decimal CashReceived { get; set; }
        public decimal Change { get; set; }
        public string GCashReference { get; set; }
    }
}