using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Nethereum.Hex.HexTypes;
using System.Threading.Tasks;
using Nethereum.AspNetSample.ViewModels;

namespace Nethereum.AspNetSample.Controllers
{
    public class HomeController : Controller
    {
        

        public HomeController()
        {
           
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(FaucetViewModel faucetViewModel)
        {
            if (ModelState.IsValid)
            {
                var privateKey = "0xb5b1870957d373ef0eeffecc6e4812c0fd08f554b37b233526acc331bf1544f7";
                var ethereumAddress = "0x12890d2cce102216644c59dae5baed380d84830c";
                var maxAmountToFund = 100;
                var amountToFund = 10;

                var account = new Account(privateKey);
                var web3 = new Web3.Web3(account);

                var balance = await web3.Eth.GetBalance.SendRequestAsync(faucetViewModel.Address);
                if (Web3.Web3.Convert.FromWei(balance.Value) > maxAmountToFund)
                {
                    ModelState.AddModelError("address", "Account cannot be funded, already has more than " + maxAmountToFund + " ether");
                    return View(faucetViewModel);
                }

                var txnHash = await web3.Eth.TransactionManager.SendTransactionAsync(account.Address, faucetViewModel.Address, new HexBigInteger(Web3.Web3.Convert.ToWei(amountToFund)));
                faucetViewModel.TransactionHash = txnHash;
                faucetViewModel.Amount = amountToFund;

                return View(faucetViewModel);
            }
            else
            {
                return View("Index");
            }
        }

    }
}