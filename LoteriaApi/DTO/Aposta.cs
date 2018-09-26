using System;
using System.Collections.Generic;

namespace LoteriaEntities.DTO
{
    public class Aposta
    {
        public IRegrasLoteria Tipo { get; set; }

        public HashSet<int> NumerosJogados { get; set; }

        public int ID { get; set; }

        public DateTime DataAposta { get; set; }

        public int Concurso { get; set; }

        public int Acertos { get; set; }
    }
}