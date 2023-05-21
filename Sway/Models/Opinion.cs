using System.ComponentModel.DataAnnotations;

namespace Sway.Models
{
    public class Opinion
    {
        public int ID { get; set; }

        [Display(Name = "Opinion")]
        [Required(ErrorMessage = "You can't leave Name blank.")]
        public string oName { get; set; }

        [Display(Name = "Opinion Rating")]
        [Range(0, 1)]
        public double oSentiment { get; set; }
        [Display(Name = "Opinion Positive Rating")]
        [Range(0, 1)]
        public double oPosSentiment { get; set; }

        [Display(Name = "Opinion Negative Rating")]
        [Range(0, 1)]
        public double oNegSentiment { get; set; }


        [Required(ErrorMessage = "Opinion must belong to a Phrase")]
        [Display(Name = "Phrase")]
        public int PhraseID { get; set; }
        public Phrase Phrase { get; set; }

       

        public ICollection<Assessment> Assessments { get; set; } = new HashSet<Assessment>();
    }
}
