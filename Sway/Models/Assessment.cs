using System.ComponentModel.DataAnnotations;

namespace Sway.Models
{
    public class Assessment
    {
        public int ID { get; set; }

        [Display(Name = "Assessment")]
        [Required(ErrorMessage = "You can't leave Name blank.")]
        public string aName { get; set; }

        [Display(Name = "Assessment Rating")]
        [Range(0, 1)]
        public double aSentiment { get; set; }
        [Display(Name = "Assessment Positive Rating")]
        [Range(0, 1)]
        public double aPosSentiment { get; set; }

        [Display(Name = "Assessment Negative Rating")]
        [Range(0, 1)]
        public double aNegSentiment { get; set; }


        [Required(ErrorMessage = "Assessment must belong to a Opinion")]
        [Display(Name = "Opinion")]
        public int OpinionID { get; set; }
        public Opinion Opinion { get; set; }
    }
}


