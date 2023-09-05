using Engine.Models;
using Engine.Services;

namespace TestEngine.Services
{
    [TestClass]
    public class TestCombatService
    {

        [TestMethod]
        public void Test_FirstAttacker()
        {
            // Player and monster with dexterity 12
            Player player = new Player("", 0, "", 0, 0, 18, 0);
            Monster monster = new Monster(0, "", "", 0, null, 12, 0, 0);
            Combatant result = CombatService.FirstAttacker(player, monster);
        }
    }
}
