using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StickerSwap.Services
{
    public class HtmlTemplate
    {
        public static string MakeBody(string content)
        {
            return $"<img src='https://stickerswap.io/imgs/logo.png' style='float:left;margin-right:20px'/><h2>Sticker Swap</h2><p>{content}</p><br/>This email was sent by <a href='https://stickerswap.io'>Sticker Swap</a>";
        }
    }
}
