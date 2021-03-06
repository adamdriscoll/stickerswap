﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace StickerSwap.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Route("invite")]
        public IActionResult Invite()
        {
            return View("InvitePage");
        }

        public IActionResult ConfirmEmail()
        {
            return View();
        }
    }
}