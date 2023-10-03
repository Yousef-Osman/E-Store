namespace E_Store.Models.Entities;

public class BaseEntity
{
    public BaseEntity()
    {
        Created = DateTime.Now;
        LastModified = DateTime.Now;
    }

    public DateTime Created { get; set; }
    public DateTime? LastModified { get; set; }
    public DateTime? Deleted { get; set; }
    public bool IsDeleted { get; set; } = false;
}
