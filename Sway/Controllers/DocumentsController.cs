using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using Azure.AI.TextAnalytics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sway.Data;
using Sway.Models;
using System.Linq;

/*using HtmlAgilityPack;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net;
using System.Text;
using System.IO;*/

namespace Sway.Controllers
{
    public class DocumentsController : Controller
    {
        private readonly SwayContext _context;

        // Please don't abuse below keys, the only other option is whitelisting each device that wants to run it
        //static string languageKey = Environment.GetEnvironmentVariable("LANGUAGE_KEY");
        //static string languageEndpoint = Environment.GetEnvironmentVariable("LANGUAGE_ENDPOINT");
        static string languageKey = "";
        static string languageEndpoint = "https://sway.cognitiveservices.azure.com/";

        private static readonly AzureKeyCredential credentials = new AzureKeyCredential(languageKey);
        private static readonly Uri endpoint = new Uri(languageEndpoint);

        public static List<string> keyPhrase = new List<string>();

        public static List<string> docList = new List<string>();

        public static List<string> phrList = new List<string>();


        public static List<Opinion> docOpList = new List<Opinion>();
        public static List<string> docOpPhraseList = new List<string>();


        public DocumentsController(SwayContext context)
        {
            _context = context;
        }


        public async Task<ActionResult> KeyPhraseAnalysis(TextAnalyticsClient client, string givenText)
        {
            var response = client.ExtractKeyPhrases(givenText);

            

            foreach (string keyphrase in response.Value)
            {
               keyPhrase.Add(keyphrase);
            }
            await _context.SaveChangesAsync();

            return Ok(response);
        }


        private static async Task<string> CallUrl(string fullUrl)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetStringAsync(fullUrl);
            return response;
        }

        public async Task<ActionResult> SentimentAnalysis(TextAnalyticsClient client, List<string> scraped, string givenName)
        {
            

            AnalyzeSentimentResultCollection reviews = client.AnalyzeSentimentBatch(scraped, options: new AnalyzeSentimentOptions()
            {
                IncludeOpinionMining = true
            });
            
            foreach (AnalyzeSentimentResult review in reviews)
            {
                
                Document document = new Document
                {
                    dName = givenName,
                    docSentiment = Convert.ToString(review.DocumentSentiment.Sentiment),
                    docPosSentiment = review.DocumentSentiment.ConfidenceScores.Positive,
                    docNegSentiment = review.DocumentSentiment.ConfidenceScores.Negative,
                    docNeutralSentiment = review.DocumentSentiment.ConfidenceScores.Neutral
                };

                _context.Documents.Add(document);
                ViewBag.Documents += document;

                foreach (SentenceSentiment sentence in review.DocumentSentiment.Sentences)
                {
                    Phrase phrase = new Phrase
                    {
                        DocumentID = document.ID,
                        pName = sentence.Text,
                        sentiment = Convert.ToDouble(sentence.Sentiment),
                        posSentiment = sentence.ConfidenceScores.Positive,
                        negSentiment = sentence.ConfidenceScores.Negative,
                        neutralSentiment = sentence.ConfidenceScores.Neutral
                    };


                    document.Phrases.Add(phrase);
                    ViewBag.Phrases += document.ID;
                    ViewBag.Phrases += phrase;

                    double pos = ((sentence.ConfidenceScores.Negative + sentence.ConfidenceScores.Positive + sentence.ConfidenceScores.Neutral) / 3);
                    ViewBag.Positivity = pos;

                    foreach (SentenceOpinion sentenceOpinion in sentence.Opinions)
                    {
                        Opinion opinion = new Opinion
                        {
                            PhraseID = phrase.ID,
                            oName = sentenceOpinion.Target.Text,
                            oSentiment = Convert.ToDouble(sentenceOpinion.Target.Sentiment),
                            oPosSentiment = Convert.ToDouble(sentenceOpinion.Target.ConfidenceScores.Positive),
                            oNegSentiment = Convert.ToDouble(sentenceOpinion.Target.ConfidenceScores.Negative)
                        };
                        phrase.Opinions.Add(opinion);

                        foreach (AssessmentSentiment assessment in sentenceOpinion.Assessments)
                        {
                            Assessment oAssessment = new Assessment
                            {
                                OpinionID = opinion.ID,
                                aName = assessment.Text,
                                aSentiment = Convert.ToDouble(assessment.Sentiment),
                                aPosSentiment = Convert.ToDouble(assessment.ConfidenceScores.Positive),
                                aNegSentiment = Convert.ToDouble(assessment.ConfidenceScores.Negative)
                            };
                            ViewBag.Assessments += oAssessment;
                            opinion.Assessments.Add(oAssessment);
                        }
                    }
                }

            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Documents");

        }


        // GET: Documents
        public async Task<IActionResult> Index()
        {
            
            ViewBag.KeyPhrase = keyPhrase;
            return View(await _context.Documents.ToListAsync());
        }

        public async Task<IActionResult> Scrape(string givenName, string givenText /*, string fullUrl*/)
        {
            var client = new TextAnalyticsClient(endpoint, credentials);
            /*givenText = givenText.Replace("!", ".");
            givenText = givenText.Replace("?", ".");
            string[] textList = givenText.Split('.',StringSplitOptions.RemoveEmptyEntries);*/
            List<string> scraped = new List<string>();
            
            //below was for webscraper
            //string fullUrl = givenURL
            //await CallUrl(fullUrl);


            //foreach(string s in textList) { scraped.Add(s); }
            scraped.Add(givenText);
            
            await SentimentAnalysis(client, scraped, givenName);
            await KeyPhraseAnalysis(client, givenText);
            return RedirectToAction("Index", "Documents");
        }


        // GET: Documents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Documents == null)
            {
                return NotFound();
            }
            docList.Clear();
            var doc = _context.Documents
                .Include(i => i.Phrases).ThenInclude(i => i.Opinions).ThenInclude(i => i.Assessments)
                .AsNoTracking();

            var phr = _context.Phrases
                .Include(i => i.Opinions).ThenInclude(i => i.Assessments)
                .AsNoTracking();

            foreach (var phru in phr)
            {
                if(id == phru.DocumentID)
                docList.Add(phru.pName);
            };

            ViewBag.docList = docList;
            var document = await _context.Documents
                .FirstOrDefaultAsync(m => m.ID == id);
            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        // GET: Opinions/5
        public async Task<IActionResult> Opinions(int? id)
        {
            docOpList.Clear();
            docOpPhraseList.Clear();
           
            if (id == null || _context.Documents == null)
            {
                return NotFound();
            }
            var doc = _context.Documents
                .Include(i => i.Phrases).ThenInclude(p => p.Opinions).ThenInclude(k => k.Assessments)
                .AsNoTracking();

            var phr = _context.Phrases
                .Include(i => i.Opinions).ThenInclude(i => i.Assessments)
                .AsNoTracking();

            var op = _context.Opinion
                .Include(i => i.Assessments)
                .Include(i => i.Phrase)
                .AsNoTracking();

            phr = phr.Where(p => p.DocumentID == id);
            
            foreach (var phrase in phr)
            {
                foreach (var opu in op)
                {
                    if (phrase.ID == opu.PhraseID)
                    {
                        docOpList.Add(opu);
                        

                        docOpPhraseList.Add(phrase.pName);
                    }
                }
            }
           

            ViewBag.docOpList = docOpList;
            ViewBag.docOpPhraseList = docOpPhraseList;

            var document = await _context.Documents
                .FirstOrDefaultAsync(m => m.ID == id);
            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }


        // GET: Documents/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Documents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,dName,docSentiment,docPosSentiment,docNegSentiment,docNeutralSentiment")] Document document)
        {
            if (ModelState.IsValid)
            {
                _context.Add(document);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(document);
        }

        // GET: Documents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Documents == null)
            {
                return NotFound();
            }

            var document = await _context.Documents.FindAsync(id);
            if (document == null)
            {
                return NotFound();
            }
            return View(document);
        }

        // POST: Documents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,dName,docSentiment,docPosSentiment,docNegSentiment,docNeutralSentiment")] Document document)
        {
            if (id != document.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(document);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DocumentExists(document.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(document);
        }

        // GET: Documents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Documents == null)
            {
                return NotFound();
            }

            var document = await _context.Documents
                .FirstOrDefaultAsync(m => m.ID == id);
            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        // POST: Documents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Documents == null)
            {
                return Problem("Entity set 'SwayContext.Documents'  is null.");
            }
            var document = await _context.Documents.FindAsync(id);
            if (document != null)
            {
                _context.Documents.Remove(document);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DocumentExists(int id)
        {
            return _context.Documents.Any(e => e.ID == id);
        }
    }
}
