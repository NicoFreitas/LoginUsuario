using System.Collections.Generic;
using System.Data;

namespace LoginUsuario.Models
{
    public class Login
    {

        public static DataTable RetornaLogin(string Login, string Senha)
        {

            DataTable resultado = new DataTable();

            string query = "SELECT LOGIN,SENHA FROM LOGUSER WHERE LOGIN = '" + Login + "' and SENHA = '" + Senha + "'";

            using (Conexao conn = new Conexao())
            {

                resultado = conn.executeSelect(query);

            }

            return resultado;

        }

        public static List<dynamic> CadastraNovoUsuario(string nome, string email, string login, string senha)
        {

            List<dynamic> user = new List<dynamic>();


            string query = "  INSERT INTO LOGUSER (NOME, LOGIN, SENHA, EMAIL)" +
                           "  VALUES('" + nome + "','" + login + "','" + senha + "','" + email + "')";

            using (Conexao conn = new Conexao())
            {


                user = conn.executeSelectList(query);
            }

            return user;

        }

        public static List<dynamic> AlterarSenhaUsuario(string login, string senha)
        {

            List<dynamic> novaSenha = new List<dynamic>();


            string query = @" UPDATE LOGUSER " +
                            " SET SENHA = '" + senha + "' " +
                            " WHERE LOGIN = '" + login + "'";

            using (Conexao conn = new Conexao())
            {
                novaSenha = conn.executeSelectList(query);
            }

            return novaSenha;
        }


    }

}
