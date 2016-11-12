using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebprosjektBankOblig.DAL;
using WebprosjektBankOblig.Models;

namespace WebprosjektBankOblig.BLL
{
    public class KontoBLL : BLL.IKontoBLL
    {
        private IKontoRepository _repository;

        public KontoBLL()
        {
            _repository = new KontoRepository();
        }

        public KontoBLL(IKontoRepository stub)
        {
            _repository = stub;
        }
        public List<Konto> hentKontoer(string pn)
        {
            return _repository.hentKontoer(pn);
        }

        public List<Konto> hentKontoer()
        {
            return _repository.hentKontoer();
        }

        public Konto hentKonto(string kontonr)
        {
            return _repository.hentKonto(kontonr);
        }

    }
}