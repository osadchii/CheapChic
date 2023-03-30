using System.ComponentModel.DataAnnotations.Schema;
using CheapChic.Data.Constants;

namespace CheapChic.Data.Attributes;

public class CheapChicTableAttribute : TableAttribute
{
    public CheapChicTableAttribute(string name) : base(name)
    {
        Schema = DatabaseConstant.DefaultSchema;
    }
}