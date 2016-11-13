using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebprosjektBankOblig.BLL;
using WebprosjektBankOblig.Models;
using WebprosjektBankOblig.DAL;
using System.Web.Mvc;
using WebprosjektBankOblig.Controllers;

namespace WebprosjektBankOblig.Tests
{
    [TestClass()]
    public class AdminControllerTests
    {
        [TestMethod]
        public void Liste()
        {
            // Arrange
            var controller = new AdminController(new AdminBLL(new AuthRepositoryStub(), new AdminRepositoryStub()), new AuthBLL(), new KontoBLL(), new BetalingBLL());
            // uten test : var controller = new PersonController();
            var forventetResultat = new List<Kunde>();
            var kunde = new Kunde()
            {
                Id = 1,
                Navn = "Per",
                Personnummer = "12345678912",
                Adresse = "Osloveien 82",
                Tlf = "12345678",
            };
            forventetResultat.Add(kunde);
            forventetResultat.Add(kunde);
            forventetResultat.Add(kunde);

            // Act
            var actionResult = (ViewResult)controller.Betalinger();
            var resultat = (List<Kunde>)actionResult.Model;
            // Assert

            Assert.AreEqual(actionResult.ViewName, "");

            for (var i = 0; i < resultat.Count; i++)
            {
                Assert.AreEqual(forventetResultat[i].Id, resultat[i].Id);
                Assert.AreEqual(forventetResultat[i].Navn, resultat[i].Navn);
                Assert.AreEqual(forventetResultat[i].Personnummer, resultat[i].Personnummer);
                Assert.AreEqual(forventetResultat[i].Adresse, resultat[i].Adresse);
                Assert.AreEqual(forventetResultat[i].Tlf, resultat[i].Tlf);
            }
            /*
            Det som kommer under er bare for å vise hva Assert.IsTrue kan gjøre (dvs alt!)
            string forventet1 = "Her er en mulighet";
            string forventet2 = "Her er en mulighet til";
            string virkelig = "Her er en mulighet";
            if (virkelig == forventet1 || virkelig==forventet2)
                test = true;
            else 
                test = false;
            Assert.IsTrue(test);
             
             */
        }
        [TestMethod()]
        public void AdminControllerTest()
        {

        }

        [TestMethod()]
        public void AdminControllerTest1()
        {

        }

        [TestMethod()]
        public void IndexTest()
        {

        }

        [TestMethod()]
        public void KunderTest()
        {

        }

        [TestMethod()]
        public void KontoerTest()
        {

        }

        [TestMethod()]
        public void BetalingerTest()
        {

        }

        [TestMethod()]
        public void LoginTest()
        {

        }
    }
}