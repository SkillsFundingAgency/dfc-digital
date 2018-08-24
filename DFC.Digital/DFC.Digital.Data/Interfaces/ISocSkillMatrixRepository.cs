using DFC.Digital.Data.Model;
using System.Collections.Generic;

namespace DFC.Digital.Data.Interfaces
{
    public interface ISocSkillMatrixRepository
    {
        void UpsertSocSkillMatrix(SocSkillMatrix socSkillMatrix);

        IEnumerable<SocSkillMatrix> GetSocSkillMatrices();
    }
}
