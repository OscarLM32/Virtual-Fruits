using UnityEditor.Build.Pipeline.Tasks;

namespace DynamicDifficulty.Skillcalculator
{
    public interface ISkillCalculator
    { 
        public Difficulty GetPlayerSkillLevel(float skillParameter);
        public Difficulty CalculateEnemyDifficulty(float difficultyParameter);
    }
}
