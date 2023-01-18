using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;

namespace SGDE.ReadingCertifications.Models
{
    public class Partida
    {
        private const string ONLY_DIGITS = "^[0-9]+$";
        private const string DIGITS_POINT_DIGITS = "^(\\d+)\\.?(\\d*$)";

        public DataRow Row { get; }
        public int? OrderReading { get; set; }
        public string Orden { get; set; }
        public string NombreCapitulo { get; set; }
        public string NombreSubCapitulo { get; set; }
        public string Descripcion { get; set; }
        public string Unidades { get; set; }
        public double? ImporteCertificadoAnterior { get; set; }
        public double? ImporteCertificadoActual { get; set; }
        public double? ImporteTotal { get; set; }
        public double? Presupuesto { get; set; }
        public double? PorcentajeCertificado { get; set; }
        public string Type { get; } = "INVALID";

        public bool IsCapitulo => !string.IsNullOrEmpty(NombreCapitulo) && string.IsNullOrEmpty(NombreSubCapitulo);
        public bool IsSubCapitulo => !string.IsNullOrEmpty(NombreSubCapitulo);

        public List<Partida> SubCapitulos { get; set; }

        public Partida(DataRow dataRow, int? orderReading = null)
        {
            Row = dataRow;
            OrderReading = orderReading;

            var cell = dataRow[2].ToString().Trim();
            var regexOnlyDigits = Regex.Match(cell, ONLY_DIGITS, RegexOptions.IgnoreCase);
            var cell1 = dataRow[3].ToString().Trim();
            var regexDigitsPointDigits = Regex.Matches(cell1, DIGITS_POINT_DIGITS, RegexOptions.IgnoreCase);
            var cell2 = dataRow[5].ToString().Trim();

            // Is Capitulo
            if (regexOnlyDigits.Success &&
                regexDigitsPointDigits.Count == 0 &&
                !string.IsNullOrEmpty(cell2))
            {
                Type = "CAPITULO";
                NombreCapitulo = cell;
                Descripcion = cell2;
                var cell3 = dataRow[11].ToString()
                    .Replace(",", ".")
                    .Trim();
                if (double.TryParse(cell3, NumberStyles.Any, CultureInfo.InvariantCulture, out double valueCell))
                    ImporteCertificadoActual = valueCell;

                cell3 = dataRow[15].ToString()
                    .Replace(",", ".")
                    .Trim();
                if (double.TryParse(cell3, NumberStyles.Any, CultureInfo.InvariantCulture, out valueCell))
                    ImporteCertificadoAnterior = valueCell;

                cell3 = dataRow[17].ToString()
                    .Replace(",", ".")
                    .Trim();
                if (double.TryParse(cell3, NumberStyles.Any, CultureInfo.InvariantCulture, out valueCell))
                    ImporteTotal = valueCell;

                cell3 = dataRow[19].ToString()
                    .Replace(",", ".")
                    .Trim();
                if (double.TryParse(cell3, NumberStyles.Any, CultureInfo.InvariantCulture, out valueCell))
                    Presupuesto = valueCell;

                cell3 = dataRow[20].ToString()
                    .Replace(",", ".")
                    .Replace("%", string.Empty)
                    .Trim();
                if (double.TryParse(cell3, NumberStyles.Any, CultureInfo.InvariantCulture, out valueCell))
                    PorcentajeCertificado = valueCell;

                SubCapitulos = new List<Partida>();
            }

            cell2 = dataRow[8].ToString().Trim();
            // Is SubCapitulo
            if (!regexOnlyDigits.Success &&
                regexDigitsPointDigits.Count > 0 &&
                !string.IsNullOrEmpty(cell2))
            {
                Type = "SUBCAPITULO";
                NombreCapitulo = regexDigitsPointDigits[0].Groups[1].Value;
                NombreSubCapitulo = regexDigitsPointDigits[0].Groups[2].Value;
                Descripcion = cell2;

                var cell3 = dataRow[1].ToString()
                    .Trim();
                Orden = cell3;

                cell3 = dataRow[7].ToString()
                    .Trim();
                Unidades = cell3;    

                cell3 = dataRow[17].ToString()
                    .Replace(",", ".")
                    .Trim();
                if (double.TryParse(cell3, NumberStyles.Any, CultureInfo.InvariantCulture, out double valueCell))
                    ImporteCertificadoActual = valueCell;

                cell3 = dataRow[18].ToString()
                    .Replace(",", ".")
                    .Trim();
                if (double.TryParse(cell3, NumberStyles.Any, CultureInfo.InvariantCulture, out valueCell))
                    ImporteCertificadoAnterior = valueCell;

                cell3 = dataRow[20].ToString()
                    .Replace(",", ".")
                    .Trim();
                if (double.TryParse(cell3, NumberStyles.Any, CultureInfo.InvariantCulture, out valueCell))
                    ImporteTotal = valueCell;

                cell3 = dataRow[24].ToString()
                    .Replace(",", ".")
                    .Replace("%", string.Empty)
                    .Trim();
                if (double.TryParse(cell3, NumberStyles.Any, CultureInfo.InvariantCulture, out valueCell))
                    PorcentajeCertificado = valueCell;
            }
        }

        public override string ToString()
        {
            if (IsCapitulo)
                return $"({OrderReading}){Type}[{NombreCapitulo}]: {Descripcion}";

            if (IsSubCapitulo)
                return $"({Orden}){Type}[{NombreCapitulo}][{NombreSubCapitulo}]: {Descripcion}";

            return $"({OrderReading}){Type}";
        }
    }
}
