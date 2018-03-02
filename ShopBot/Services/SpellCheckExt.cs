using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.SpellCheck;
using Microsoft.Azure.CognitiveServices.SpellCheck.Models;

namespace ShopBot.Services
{
    public static class SpellCheckExt
    {
        public static async Task<string> SpellCheck(this SpellCheckAPI api, string spellcheck)
        {
            return (await api.SpellCheckerWithHttpMessagesAsync(text: spellcheck, mode: "proof"))
                .Body
                .FlaggedTokens
                .Where(token => token.Suggestions.First().Score > 0.70)
                .Select<SpellingFlaggedToken, Func<string, string>>(token => 
                    (value => value.Replace(token.Token, token.Suggestions.First().Suggestion)))
                .DefaultIfEmpty(_ => _)
                .Aggregate((a, b) => s => a(b(s)))
                .Invoke(spellcheck);
        }
    }
}