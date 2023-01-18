using System.Collections.Generic;
using System.Linq;

namespace SGDE.ReadingCertifications.Models
{
    public class CertificarionXLS
    {
        public List<Partida> Partidas { get; } = new List<Partida>();
        public List<Partida> Capitulos { get; } = new List<Partida>();
        public List<Partida> SubCapitulos { get; } = new List<Partida>();

        public CertificarionXLS()
        {
        }

        public void AddPartida(Partida partida)
        {
            Partidas.Add(partida);

            if (partida.IsCapitulo)
                Capitulos.Add(partida);

            if (partida.IsSubCapitulo)
            {
                SubCapitulos.Add(partida);
                var findCapitulo = Capitulos.FirstOrDefault(x => x.NombreCapitulo == partida.NombreCapitulo);
                if (findCapitulo != null)
                    findCapitulo.SubCapitulos.Add(partida);
            }
        }
    }
}
