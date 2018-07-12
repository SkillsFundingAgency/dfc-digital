using DFC.Digital.Data.Model;

namespace DFC.Digital.Data.Interfaces
{
    public interface IOnetRepository
    {
        RepoActionResult UpsertOnetSkill(OnetSkill onetSkill);
    }
}
