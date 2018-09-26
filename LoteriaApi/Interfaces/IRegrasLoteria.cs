using System.Collections.Generic;

namespace LoteriaEntities.DTO
{
    public interface IRegrasLoteria
    {
        int NumeroMin { get;  }
        int NumeroMax { get; }
        int QtdSorteada { get;  }
        List<int> Ganhadores { get; }
    }
}