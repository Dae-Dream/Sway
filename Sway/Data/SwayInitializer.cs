namespace Sway.Data
{
    public static class SwayInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            SwayContext context = applicationBuilder.ApplicationServices.CreateScope()
                .ServiceProvider.GetRequiredService<SwayContext>();
            if(!context.Documents.Any())
            {
                context.Documents.AddRange(
                    new Models.Document
                    {
                        dName = "Classes or 1322 or something",
                        docSentiment = "Mixed",
                        docNegSentiment = 0.1,
                        docPosSentiment = 0.7,
                        docNeutralSentiment = 0.5
                    },
                    new Models.Document
                    {
                        dName = "Random Phrases for here",
                        docSentiment = "Mixed",
                        docNegSentiment = 0.2,
                        docPosSentiment = 0.7,
                        docNeutralSentiment = 0.5
                    },
                    new Models.Document
                    {
                        dName = "Testing Class",
                        docSentiment = "Positive",
                        docNegSentiment = 0.1,
                        docPosSentiment = 0.7,
                        docNeutralSentiment = 0.5
                    }

                    ) ;
                context.SaveChanges();
            }
            if (!context.Phrases.Any())
            {
                context.Phrases.AddRange(
                    new Models.Phrase
                    {
                        pName = "1322",
                        sentiment = 0.3,
                        negSentiment = 0.2,
                        posSentiment = 0.6,
                        neutralSentiment = 0.5,
                        DocumentID = context.Documents.FirstOrDefault(d => d.dName == "Classes or 1322 or something").ID
                    },
                    new Models.Phrase
                    {
                        pName = "1322",
                        sentiment = 0.3,
                        negSentiment = 0.2,
                        posSentiment = 0.6,
                        neutralSentiment = 0.5,
                        DocumentID = context.Documents.FirstOrDefault(d => d.dName == "Random Phrases for here").ID

                    },
                    new Models.Phrase
                    {
                         pName = "1322",
                        sentiment = 0.3,
                        negSentiment = 0.2,
                        posSentiment = 0.6,
                        neutralSentiment = 0.5,
                        DocumentID = context.Documents.FirstOrDefault(d => d.dName == "Testing Class").ID
                    }
                    );
                context.SaveChanges();


            }
        }
    }
}
