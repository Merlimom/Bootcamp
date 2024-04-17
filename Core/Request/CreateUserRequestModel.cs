namespace Core.Request;

public class CreateUserRequestModel
{
    public DateTime RequestDate { get; set; }

    public DateTime ApprovalDate { get; set; }

    public int CurrencyId { get; set; }

    public int CustomerId { get; set; }

    public int ProductId { get; set; }

}
