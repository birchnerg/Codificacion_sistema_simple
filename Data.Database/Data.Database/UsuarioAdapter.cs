using System;
using System.Collections.Generic;
using System.Text;
using Business.Entities;
using System.Data;
using System.Data.SqlClient;

namespace Data.Database
{
    public class UsuarioAdapter:Adapter
    {
        #region DatosEnMemoria
        // Esta regi�n solo se usa en esta etapa donde los datos se mantienen en memoria.
        // Al modificar este proyecto para que acceda a la base de datos esta ser� eliminada
        private static List<Usuario> _Usuarios;

        private static List<Usuario> Usuarios
        {
            get
            {
                if (_Usuarios == null)
                {
                    _Usuarios = new List<Business.Entities.Usuario>();
                    Business.Entities.Usuario usr;
                    usr = new Business.Entities.Usuario();
                    usr.ID = 1;
                    usr.State = Business.Entities.BusinessEntity.States.Unmodified;
                    usr.Nombre = "Casimiro";
                    usr.Apellido = "Cegado";
                    usr.NombreUsuario = "casicegado";
                    usr.Clave = "miro";
                    usr.Email = "casimirocegado@gmail.com";
                    usr.Habilitado = true;
                    _Usuarios.Add(usr);

                    usr = new Business.Entities.Usuario();
                    usr.ID = 2;
                    usr.State = Business.Entities.BusinessEntity.States.Unmodified;
                    usr.Nombre = "Armando Esteban";
                    usr.Apellido = "Quito";
                    usr.NombreUsuario = "aequito";
                    usr.Clave = "carpintero";
                    usr.Email = "armandoquito@gmail.com";
                    usr.Habilitado = true;
                    _Usuarios.Add(usr);

                    usr = new Business.Entities.Usuario();
                    usr.ID = 3;
                    usr.State = Business.Entities.BusinessEntity.States.Unmodified;
                    usr.Nombre = "Alan";
                    usr.Apellido = "Brado";
                    usr.NombreUsuario = "alanbrado";
                    usr.Clave = "abrete sesamo";
                    usr.Email = "alanbrado@gmail.com";
                    usr.Habilitado = true;
                    _Usuarios.Add(usr);

                }
                return _Usuarios;
            }
        }
        #endregion

        public List<Usuario> GetAll()
        {
            //return new List<Usuario>(Usuarios);
            List<Usuario> usuarios = new List<Usuario>();

            this.OpenConnection();

            //Objeto SqlCommand para la sentencia SQL que se va a ejecutar
            SqlCommand cmdUsuarios = new SqlCommand("SELECT * FROM usuarios", sqlConn);

            //DataReader para recuperar los datos de la DB
            SqlDataReader drUsuario = cmdUsuarios.ExecuteReader();

            //Read() lee una fila de las devueltas por el commandSql, devuelve true mientras pueda leer datos y avanza 
            //a la siguiente fila
            while (drUsuario.Read())
            {
                //Creamos un obj Usuario de la capa de entidades para copiar los datos del DataReader
                Usuario usr = new Usuario();

                usr.ID = (int)drUsuario["id_usuario"];
                usr.NombreUsuario = (string)drUsuario["nombre_usuario"];
                usr.Clave = (string)drUsuario["clave"];
                usr.Habilitado = (bool)drUsuario["habilitado"];
                usr.Nombre = (string)drUsuario["nombre"];
                usr.Apellido = (string)drUsuario["apellido"];
                usr.Email = (string)drUsuario["email"];

                usuarios.Add(usr);
            }
            drUsuario.Close();
            this.CloseConnection();

            return usuarios;
        }

        public Business.Entities.Usuario GetOne(int ID)
        {
            return Usuarios.Find(delegate(Usuario u) { return u.ID == ID; });
        }

        public void Delete(int ID)
        {
            Usuarios.Remove(this.GetOne(ID));
        }

        public void Save(Usuario usuario)
        {
            if (usuario.State == BusinessEntity.States.New)
            {
                int NextID = 0;
                foreach (Usuario usr in Usuarios)
                {
                    if (usr.ID > NextID)
                    {
                        NextID = usr.ID;
                    }
                }
                usuario.ID = NextID + 1;
                Usuarios.Add(usuario);
            }
            else if (usuario.State == BusinessEntity.States.Deleted)
            {
                this.Delete(usuario.ID);
            }
            else if (usuario.State == BusinessEntity.States.Modified)
            {
                Usuarios[Usuarios.FindIndex(delegate(Usuario u) { return u.ID == usuario.ID; })]=usuario;
            }
            usuario.State = BusinessEntity.States.Unmodified;            
        }
    }
}
