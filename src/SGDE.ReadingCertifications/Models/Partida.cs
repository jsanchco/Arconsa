using HtmlAgilityPack;
using SGDE.ReadingCertifications.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SGDE.ReadingCertifications.Models
{
    public class Partida
    {
        private const string ONLY_DIGITS = "^[0-9]+$";
        private const string DIGITS_POINT_DIGITS = "^(\\d+)\\.?(\\d*$)";

        public List<HtmlNode> Nodes { get; }
        public string Codigo { get; set; }
        public string Orden { get; set; }
        public string Descripcion { get; set; }
        public string Unidades { get; set; }

        public string ImporteUnidadText { get; set; }

        public double? ImporteUnidad
        {
            get
            {
                if (string.IsNullOrEmpty(ImporteUnidadText))
                    return null;

                var text = ImporteUnidadText
                    .Trim()
                    .Replace("(", string.Empty)
                    .Replace(")", string.Empty)
                    .Trim();
                var split = text.Split();
                if (split.Length != 2)
                    return null;

                return split[0].ToDouble();
            }
        }
        
        public double? UnidadesTotalOrigen
        {
            get
            {
                var value = UnidadesCertificacionAnterior + UnidadesCertificacionActual;
                if (!value.HasValue)
                    return null;

                return Math.Round(value.Value, 2);
            }
        }
                        

        public double? UnidadesCertificacionAnterior { get; set; }
        public double? ImporteCertificadoAnterior
        {
            get
            {
                if (IsCapitulo)
                {
                    var value = SubCapitulos
                        .Where(x => x.ImporteCertificadoAnterior != null)
                        .Sum(x => x.ImporteCertificadoAnterior);
                    if (!value.HasValue)
                        return null;

                    return Math.Round(value.Value, 2);
                }

                if (IsSubCapitulo)
                {
                    var value = UnidadesCertificacionAnterior * ImporteUnidad;
                    if (!value.HasValue)
                        return null;

                    return Math.Round(value.Value, 2);
                }

                return null;
            }
        }


        public double? UnidadesCertificacionActual { get; set; }
        public double? ImporteCertificadoActual
        {
            get
            {
                if (IsCapitulo)
                {
                    var value = SubCapitulos
                        .Where(x => x.ImporteCertificadoActual != null)
                        .Sum(x => x.ImporteCertificadoActual);
                    if (!value.HasValue)
                        return null;

                    return Math.Round(value.Value, 2);
                }

                if (IsSubCapitulo)
                {
                    var value = UnidadesCertificacionActual * ImporteUnidad;
                    if (!value.HasValue)
                        return null;

                    return Math.Round(value.Value, 2);
                }

                return null;
            }
        }

        public double? ImporteTotal
        {
            get
            {
                if (IsCapitulo)
                {
                    var value = SubCapitulos
                        .Where(x => x.ImporteTotal != null)
                        .Sum(x => x.ImporteTotal);
                    if (!value.HasValue)
                        return null;

                    return Math.Round(value.Value, 2);
                }

                if (IsSubCapitulo)
                {
                    var value = (UnidadesCertificacionAnterior + UnidadesCertificacionActual) * ImporteUnidad;
                    if (!value.HasValue)
                        return null;

                    return Math.Round(value.Value, 2);
                }

                return null;
            }
        }

        public double? UnidadesPresupuesto { get; set; }
        public double? Presupuesto
        {
            get
            {
                if (IsCapitulo)
                {
                    return PresupuestoCapitulo;
                }

                if (IsSubCapitulo)
                {
                    if (!UnidadesPresupuesto.HasValue || !ImporteUnidad.HasValue)
                        return null;

                    return Math.Round(ImporteUnidad.Value * UnidadesPresupuesto.Value, 2);
                }

                return null;
            }
        }

        public double? PorcentajeCertificado 
        {
            get
            {
                if (IsCapitulo)
                {
                    var value = SubCapitulos
                        .Where(x => x.ImporteTotal != null)
                        .Sum(x => x.ImporteTotal);
                    if (!value.HasValue)
                        return null;

                    return Math.Round(value.Value * 100 / Presupuesto.Value);
                }

                if (IsSubCapitulo)
                {
                    if (!ImporteTotal.HasValue || !Presupuesto.HasValue)
                        return null;

                    return Math.Round(ImporteTotal.Value * 100 / Presupuesto.Value);
                }

                return null;
            }
        }

        public double? PresupuestoCapitulo { get; set; }

        public string Type { get; } = "INVALID";

        public bool IsCapitulo => Type == "CAPITULO";
        public bool IsSubCapitulo => Type == "SUBCAPITULO";

        public List<Partida> SubCapitulos { get; set; }

        #region Excel

        //public Partida(DataRow dataRow, int? orderReading = null)
        //{
        //    Row = dataRow;
        //    OrderReading = orderReading;

        //    var cell = dataRow[2].ToString().Trim();
        //    var regexOnlyDigits = Regex.Match(cell, ONLY_DIGITS, RegexOptions.IgnoreCase);
        //    var cell1 = dataRow[3].ToString().Trim();
        //    var regexDigitsPointDigits = Regex.Matches(cell1, DIGITS_POINT_DIGITS, RegexOptions.IgnoreCase);
        //    var cell2 = dataRow[5].ToString().Trim();

        //    // Is Capitulo
        //    if (regexOnlyDigits.Success &&
        //        regexDigitsPointDigits.Count == 0 &&
        //        !string.IsNullOrEmpty(cell2))
        //    {
        //        Type = "CAPITULO";
        //        NombreCapitulo = cell;
        //        Descripcion = cell2;
        //        var cell3 = dataRow[11].ToString()
        //            .Replace(",", ".")
        //            .Trim();
        //        if (double.TryParse(cell3, NumberStyles.Any, CultureInfo.InvariantCulture, out double valueCell))
        //            ImporteCertificadoActual = valueCell;

        //        cell3 = dataRow[15].ToString()
        //            .Replace(",", ".")
        //            .Trim();
        //        if (double.TryParse(cell3, NumberStyles.Any, CultureInfo.InvariantCulture, out valueCell))
        //            ImporteCertificadoAnterior = valueCell;

        //        cell3 = dataRow[17].ToString()
        //            .Replace(",", ".")
        //            .Trim();
        //        if (double.TryParse(cell3, NumberStyles.Any, CultureInfo.InvariantCulture, out valueCell))
        //            ImporteTotal = valueCell;

        //        cell3 = dataRow[19].ToString()
        //            .Replace(",", ".")
        //            .Trim();
        //        if (double.TryParse(cell3, NumberStyles.Any, CultureInfo.InvariantCulture, out valueCell))
        //            Presupuesto = valueCell;

        //        cell3 = dataRow[20].ToString()
        //            .Replace(",", ".")
        //            .Replace("%", string.Empty)
        //            .Trim();
        //        if (double.TryParse(cell3, NumberStyles.Any, CultureInfo.InvariantCulture, out valueCell))
        //            PorcentajeCertificado = valueCell;

        //        SubCapitulos = new List<Partida>();
        //    }

        //    cell2 = dataRow[8].ToString().Trim();
        //    // Is SubCapitulo
        //    if (!regexOnlyDigits.Success &&
        //        regexDigitsPointDigits.Count > 0 &&
        //        !string.IsNullOrEmpty(cell2))
        //    {
        //        Type = "SUBCAPITULO";
        //        NombreCapitulo = regexDigitsPointDigits[0].Groups[1].Value;
        //        NombreSubCapitulo = regexDigitsPointDigits[0].Groups[2].Value;
        //        Descripcion = cell2;

        //        var cell3 = dataRow[1].ToString()
        //            .Trim();
        //        Orden = cell3;

        //        cell3 = dataRow[7].ToString()
        //            .Trim();
        //        Unidades = cell3;    

        //        cell3 = dataRow[17].ToString()
        //            .Replace(",", ".")
        //            .Trim();
        //        if (double.TryParse(cell3, NumberStyles.Any, CultureInfo.InvariantCulture, out double valueCell))
        //            ImporteCertificadoActual = valueCell;

        //        cell3 = dataRow[18].ToString()
        //            .Replace(",", ".")
        //            .Trim();
        //        if (double.TryParse(cell3, NumberStyles.Any, CultureInfo.InvariantCulture, out valueCell))
        //            ImporteCertificadoAnterior = valueCell;

        //        cell3 = dataRow[20].ToString()
        //            .Replace(",", ".")
        //            .Trim();
        //        if (double.TryParse(cell3, NumberStyles.Any, CultureInfo.InvariantCulture, out valueCell))
        //            ImporteTotal = valueCell;

        //        cell3 = dataRow[24].ToString()
        //            .Replace(",", ".")
        //            .Replace("%", string.Empty)
        //            .Trim();
        //        if (double.TryParse(cell3, NumberStyles.Any, CultureInfo.InvariantCulture, out valueCell))
        //            PorcentajeCertificado = valueCell;
        //    }
        //}

        #endregion

        #region HTML

        public Partida(List<HtmlNode> nodes)
        {
            Nodes = nodes;

            if (nodes.Count == 7) // Capítulos
            {
                Codigo = nodes[1].InnerText.Trim();
                Descripcion = nodes[2].InnerText.Trim();
                PresupuestoCapitulo = nodes[0].InnerText.ToDouble();

                Type = "CAPITULO";
            }

            if (nodes.Count == 14) // SubCapítulos
            {
                Codigo = nodes[0].InnerText.Trim();
                Orden = nodes[2].InnerText.Trim();
                Descripcion = nodes[1].InnerText.Trim();
                Unidades = nodes[8].InnerText.Trim();
                UnidadesPresupuesto = nodes[9].InnerText.ToDouble();
                UnidadesCertificacionAnterior = nodes[11].InnerText.ToDouble();
                UnidadesCertificacionActual = nodes[12].InnerText.ToDouble();
                ImporteUnidadText = nodes[13].InnerText.Trim();

                Type = "SUBCAPITULO";
            }
        }

        #endregion

        public override string ToString()
        {
            if (IsCapitulo)
                return $"{Type} -> ({Codigo})[{Descripcion}][{ImporteCertificadoActual}][{ImporteCertificadoAnterior}][{ImporteTotal}][{Presupuesto}][{PorcentajeCertificado}]";

            if (IsSubCapitulo)
            {
                var result = $"{Type} -> ({Orden})({Codigo})[{Unidades}][{Descripcion}][{ImporteCertificadoActual}][{ImporteCertificadoAnterior}][{ImporteTotal}][{Presupuesto}][{PorcentajeCertificado}] \r\n";
                result += $"({ImporteUnidad} {Unidades})({UnidadesCertificacionActual})({UnidadesCertificacionAnterior})({UnidadesTotalOrigen})({UnidadesPresupuesto})";

                return result;
            }

            return $"({Codigo}){Type}";
        }
    }
}
