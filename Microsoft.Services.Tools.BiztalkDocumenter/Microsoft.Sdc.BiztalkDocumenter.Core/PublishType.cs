
namespace Microsoft.Services.Tools.BiztalkDocumenter
{
    public enum PublishType : int
    {
        EntireConfiguration = 1,
        SchemaOnly = 2,
        SpecificApplication = 3,
    }

    public delegate void UpdatePercentageComplete(int percentage);
}
