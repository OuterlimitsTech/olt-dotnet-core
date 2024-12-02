using OLT.EF.Core.Services.Tests.Assets.Models;

namespace OLT.EF.Core.Services.Tests.Automapper.Models;

public class PersonDetailDto : PersonDto
{
    public DateTimeOffset Created { get; set; }
}
