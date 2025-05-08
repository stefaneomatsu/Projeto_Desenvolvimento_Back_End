using SGHSS_Uninter.Api.Atributos;
using SGHSS_Uninter.Api.Enumeradores;

namespace SGHSS_Uninter.Api.Models.Persistente
{
    public class BaseObjetoPersistente
    {
        private string _chave;

        public BaseObjetoPersistente()
        {
            InicializeObjeto();
        }

        [ColunaBD("chave")]
        public string Chave 
        {
            get { return _chave.ToUpper(); }
            set { _chave = value; }
        }

        [ColunaBD("data_criacao")]
        public DateTime DataCriacao { get; set; }

        [ColunaBD("data_alteracao")]
        public DateTime? DataAlteracao { get; set; }

        [ColunaBD("status")]
        public EnumStatusRegistro Status { get; set; }

        public virtual bool EhNovoObjeto { get; set; }

        private void InicializeObjeto()
        {
            EhNovoObjeto = true;
            Chave = Guid.NewGuid().ToString();
            DataCriacao = DateTime.UtcNow;
            DataAlteracao = DateTime.UtcNow;
            Status = EnumStatusRegistro.ATIVO;
        }

    }
}
