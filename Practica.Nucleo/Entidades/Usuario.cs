 using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Practica.Nucleo.Enumeradores;

namespace Practica.Nucleo.Entidades


{
    public class Usuario : Persistent
    {
        public override int Id { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Cuenta { get; set; }
        public Rol Rol { get; set; }
        public string Password { get; set; }
        public string Sal { get; set; }

        public static IList<Usuario> ObtenerTodos()
        {
            IList<Usuario> usuarios;
            try
            {
                using (ISession session = Persistent.SessionFactory.OpenSession())
                {
                    ICriteria crit = session.CreateCriteria(new Usuario().GetType());
                    crit.SetProjection(Projections.ProjectionList()
                        .Add(Projections.Property("Id"), "Id")
                        .Add(Projections.Property("Nombre"), "Nombre")
                        .Add(Projections.Property("Direccion"), "Direccion")
                        .Add(Projections.Property("Telefono"), "Telefono")
                        .Add(Projections.Property("Cuenta"), "Cuenta"));
                    crit.SetResultTransformer(Transformers.AliasToBean<Usuario>());
                    usuarios = crit.List<Usuario>();
                    session.Close();
                }
            } catch (Exception ex)
            {
                throw ex;
            }
            return usuarios;
        }
     

        public static Usuario ObtenerPorId(int id)
        {
            Usuario u = new Usuario();
            try
            {
                using (ISession session = Persistent.SessionFactory.OpenSession())
                {
                    ICriteria crit = session.CreateCriteria(u.GetType());
                    crit.Add(Expression.Eq("Id", id));
                    u = (crit.UniqueResult<Usuario>());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return u;
        }

        public static Usuario ObtenerPorLogin(string cuenta, string password)
        {
            Usuario u = new Usuario();
            try
            {
                using (ISession session = Persistent.SessionFactory.OpenSession())
                {
                    
                    var sal = session.CreateSQLQuery("SELECT sal FROM trackpackdb.usuario WHERE cuenta='" + cuenta + "'")
                        .UniqueResult();
                    password = Encrypt(password, sal.ToString());

                    ICriteria crit = session.CreateCriteria(u.GetType());
                    crit.Add(Expression.Eq("Cuenta", cuenta));
                    crit.Add(Expression.Eq("Password", password));
                    u = (crit.UniqueResult<Usuario>());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return u;
        }


        public static string GenerarSal()
        {
            var random = new RNGCryptoServiceProvider();

            // Tamaño de la cadena de texto
            int max_length = 15;

            // Creamos un array
            byte[] salt = new byte[max_length];

            // Generamos los bytes aleatorios
            random.GetNonZeroBytes(salt);

            // Retornamos el byte convertido a string
            return Convert.ToBase64String(salt);
        }

        public static string Encrypt(string password, string sal)
        {
            byte[] data = UTF8Encoding.UTF8.GetBytes(password);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(sal));
                using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform transform = tripDes.CreateEncryptor();
                    byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                    password = Convert.ToBase64String(results, 0, results.Length);
                    return password;

                }
            }
        }


        public static bool Guardar(int id, string nombre, string direccion, string telefono, string cuenta, int rol, string password)
        {
            bool realizado = false;
            try
            {
                //Usuario u = new Usuario();
                //if (id != 0) u = ObtenerPorId(id);
                string sal = GenerarSal();
                Usuario u = id == 0 ? new Usuario() : ObtenerPorId(id);
                u.Nombre = nombre;
                u.Direccion = direccion;
                u.Telefono = telefono;
                u.Cuenta = cuenta;
                u.Rol = (Rol)rol;
                u.Sal = sal;
                password = Encrypt(password, sal);
                if (id == 0)
                {
                    u.Password = password;
                    u.Save();
                }
                else
                {
                    u.Update();
                }

                realizado = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return realizado;
        }

        public static bool ActualizarPassword(int id, string password, string passwordValidar, string passwordNueva)
        {
            bool realizado = false;
            try
            {
                //Usuario u = new Usuario();
                //if (id != 0) u = ObtenerPorId(id);

                string salNueva = GenerarSal();
                Usuario u = ObtenerPorId(id);
                string salVieja = u.Sal;
                password = Encrypt(password, salVieja);
                if (u.Password == password && passwordNueva == passwordValidar)
                {
                    passwordNueva = Encrypt(passwordNueva, salNueva);
                    u.Sal = salNueva;
                    u.Password = passwordNueva;
                    u.Update();
                }

                realizado = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return realizado;
        }

        public static bool Borrar (int id)
        {
            bool realizado = false;
            try
            {
                Usuario u = ObtenerPorId(id);
                u.Delete();
                
                realizado = true;
            } catch (Exception ex )
            {
                throw ex;
            }

            return realizado;
        }
    }
}
