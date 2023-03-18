using System.ComponentModel.DataAnnotations.Schema;
using CheapChic.Data.Constants;

namespace CheapChic.Data.Attributes;

public class ChapChicTableAttribute : TableAttribute
{
    public ChapChicTableAttribute(string name) : base(name)
    {
        Schema = DatabaseConstant.DefaultSchema;
    }
}