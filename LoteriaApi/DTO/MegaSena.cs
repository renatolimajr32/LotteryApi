using System.Collections.Generic;

namespace LoteriaEntities.DTO
{
    public class MegaSena : IRegrasLoteria
    {
        public int NumeroMin { get { return 1; } }
        public int NumeroMax { get { return 60; } }
        public int QtdSorteada { get { return 6; } }
        public List<int> Ganhadores => new List<int>() { 4, 5, 6 };
    }
}