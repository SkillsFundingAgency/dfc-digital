namespace DFC.Digital.Service.SkillsFramework.Interface
{
    using System.Threading.Tasks;

    public interface IBusinessRuleEngine
    {
        Task<SkillsFramework> GetSkillsFrameworkFor(string socCode);
    }
}