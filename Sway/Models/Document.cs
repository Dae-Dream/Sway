using System.ComponentModel.DataAnnotations;

namespace Sway.Models
{
    public class Document
    {
        public int ID { get; set; }

        [Display(Name = "Overall Positivity")]
        public double Positivity
        {
            get { return Math.Round(Convert.ToDouble((docPosSentiment + docNegSentiment + docNeutralSentiment) / 3), 2); }
        }
    

        [Display(Name = "Document")]
        [Required(ErrorMessage = "No Document found")]
        public string dName { get; set; }

        [Display(Name = "URL")]
        public string url { get; set; }

        [Display(Name = "Document Rating")]
        [Range(0, 1)]
        public string docSentiment { get; set; }

        [Display(Name = "Document Positive Rating")]
        [Range(0, 1)]
        public double docPosSentiment { get; set; }

        [Display(Name = "Document Negative Rating")]
        [Range(0, 1)]
        public double docNegSentiment { get; set; }

        [Display(Name = "Document Neutral Rating")]
        [Range(0, 1)]
        public double docNeutralSentiment { get; set; }

        public ICollection<Phrase> Phrases { get; set; }  = new HashSet<Phrase>();
    }
}
