﻿using LeoPetri.Common.Extensions;

namespace LeoPetri.Common.Functions
{
    public static class CountryIdFunctions
    {
        public static bool IsCpfValid(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                return false;

            cpf = cpf.NumbersOnly();

            if (cpf.Length > 11)
                return false;

            while (cpf.Length != 11)
                cpf = '0' + cpf;

            var igual = true;
            for (var i = 1; i < 11 && igual; i++)
                if (cpf[i] != cpf[0])
                    igual = false;

            if (igual || cpf == "12345678909")
                return false;

            var numeros = new int[11];

            for (var i = 0; i < 11; i++)
                numeros[i] = int.Parse(cpf[i].ToString());

            var soma = 0;
            for (var i = 0; i < 9; i++)
                soma += (10 - i) * numeros[i];

            var resultado = soma % 11;

            if (resultado == 1 || resultado == 0)
            {
                if (numeros[9] != 0)
                    return false;
            }
            else if (numeros[9] != 11 - resultado)
                return false;

            soma = 0;
            for (var i = 0; i < 10; i++)
                soma += (11 - i) * numeros[i];

            resultado = soma % 11;

            if (resultado == 1 || resultado == 0)
            {
                if (numeros[10] != 0)
                    return false;
            }
            else
                if (numeros[10] != 11 - resultado)
                    return false;

            return true;
        }

        public static bool IsCnpjValid(string cnpj)
        {
            if (string.IsNullOrWhiteSpace(cnpj))
                return false;

            cnpj = cnpj.NumbersOnly();

            const string ftmt = "6543298765432";
            var digitos = new int[14];
            var soma = new int[2];
            soma[0] = 0;
            soma[1] = 0;
            var resultado = new int[2];
            resultado[0] = 0;
            resultado[1] = 0;
            var cnpjOk = new bool[2];
            cnpjOk[0] = false;
            cnpjOk[1] = false;

            try
            {
                int nrDig;
                for (nrDig = 0; nrDig < 14; nrDig++)
                {
                    digitos[nrDig] = int.Parse(
                     cnpj.Substring(nrDig, 1));
                    if (nrDig <= 11)
                        soma[0] += digitos[nrDig] *
                                   int.Parse(ftmt.Substring(
                                       nrDig + 1, 1));
                    if (nrDig <= 12)
                        soma[1] += digitos[nrDig] *
                                   int.Parse(ftmt.Substring(
                                       nrDig, 1));
                }

                for (nrDig = 0; nrDig < 2; nrDig++)
                {
                    resultado[nrDig] = soma[nrDig] % 11;
                    if ((resultado[nrDig] == 0) || (resultado[nrDig] == 1))
                        cnpjOk[nrDig] = digitos[12 + nrDig] == 0;

                    else
                        cnpjOk[nrDig] = digitos[12 + nrDig] == 11 - resultado[nrDig];

                }

                return cnpjOk[0] && cnpjOk[1];

            }
            catch
            {
                return false;
            }

        }

        public static string ToCnpjFormat(string cnpj)
        {
            return ToCnpjFormat(long.Parse(cnpj.NumbersOnly()));
        }

        public static string ToCpfFormat(string cpf)
        {
            return ToCpfFormat(long.Parse(cpf.NumbersOnly()));
        }

        public static string ToCnpjFormat(long cnpj)
        {
            return cnpj.ToString(@"00\.000\.000\/0000\-00");
        }

        public static string ToCpfFormat(long cpf)
        {
            return cpf.ToString(@"000\.000\.000\-00");
        }
    }
}
