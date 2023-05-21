using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Sway.Models
{
    public class Phrase
    {
        public int ID { get; set; }

        [Display(Name = "Phrase")]
        [Required(ErrorMessage = "You can't leave Name blank.")]
        public string pName { get; set; }

        [Display(Name = "Phrase Rating")]
        [Range(0, 1)]
        public double sentiment { get; set; }
        [Display(Name = "Phrase Positive Rating")]
        [Range(0, 1)]
        public double posSentiment { get; set; }

        [Display(Name = "Phrase Negative Rating")]
        [Range(0, 1)]
        public double negSentiment { get; set; }

        [Display(Name = "Phrase Neutral Rating")]
        [Range(0, 1)]
        public double neutralSentiment { get; set; }

        [Required(ErrorMessage = "Phrase must belong to a Document")]
        [Display(Name = "Document")]
        public int DocumentID { get; set; }
        public Document Document { get; set; }

        public ICollection<Opinion> Opinions { get; set; } = new HashSet<Opinion>();
    }
}
