using Microsoft.EntityFrameworkCore;
using ReferenceManager.Models;

namespace ReferenceManager.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        if (!await db.Papers.AnyAsync())
        {
            // &begin[Authors]
            static Affiliation Chalmers() => new() { Name = "Chalmers University of Technology", City = "Gothenburg", Country = "Sweden" };
            static Affiliation Rub() => new() { Name = "Ruhr-Universität Bochum", City = "Bochum", Country = "Germany" };

            var johan = new Author { Name = "Johan Martinson", Affiliations = [Rub()] };
            var thorsten = new Author { Name = "Thorsten Berger", Affiliations = [Rub()] };
            var mukelabai = new Author { Name = "Mukelabai Mukelabai", Affiliations = [Chalmers()] };
            var wardah = new Author { Name = "Wardah Mahmood", Affiliations = [Chalmers()] };
            var herman = new Author { Name = "Herman Jansson", Affiliations = [Chalmers()] };
            var alexandre = new Author { Name = "Alexandre Bergel", Affiliations = [new() { Name = "University of Chile", City = "Santiago", Country = "Chile" }] };
            var truong = new Author { Name = "Truong Ho-Quang", Affiliations = [Chalmers()] };
            var daniela = new Author { Name = "Daniela Lettner", Affiliations = [new() { Name = "Johannes Kepler University Linz", City = "Linz", Country = "Austria" }] };
            var julia = new Author { Name = "Julia Rubin", Affiliations = [new() { Name = "University of British Columbia", City = "Vancouver", Country = "Canada" }] };
            var paul = new Author { Name = "Paul Grünbacher", Affiliations = [new() { Name = "Johannes Kepler University Linz", City = "Linz", Country = "Austria" }] };
            var adeline = new Author { Name = "Adeline Silva", Affiliations = [] };
            var martin = new Author { Name = "Martin Becker", Affiliations = [] };
            var marsha = new Author { Name = "Marsha Chechik", Affiliations = [new() { Name = "University of Toronto", City = "Toronto", Country = "Canada" }] };
            var krzysztof = new Author { Name = "Krzysztof Czarnecki", Affiliations = [new() { Name = "University of Waterloo", City = "Waterloo", Country = "Canada" }] };
            var christian = new Author { Name = "Christian Kästner", Affiliations = [new() { Name = "Carnegie Mellon University", City = "Pittsburgh", Country = "USA" }] };
            var alexander = new Author { Name = "Alexander Dreiling", Affiliations = [new() { Name = "Philipps-Universität Marburg", City = "Marburg", Country = "Germany" }] };
            var klaus = new Author { Name = "Klaus Ostermann", Affiliations = [new() { Name = "Philipps-Universität Marburg", City = "Marburg", Country = "Germany" }] };
            var sven = new Author { Name = "Sven Apel", Affiliations = [new() { Name = "University of Passau", City = "Passau", Country = "Germany" }] };
            var don = new Author { Name = "Don Batory", Affiliations = [new() { Name = "University of Texas at Austin", City = "Austin", Country = "USA" }] };
            var gunter = new Author { Name = "Gunter Saake", Affiliations = [new() { Name = "University of Magdeburg", City = "Magdeburg", Country = "Germany" }] };
            var kyo = new Author { Name = "Kyo Kang", Affiliations = [new() { Name = "Software Engineering Institute", City = "Pittsburgh", Country = "USA" }] };
            var sholom = new Author { Name = "Sholom Cohen", Affiliations = [new() { Name = "Software Engineering Institute", City = "Pittsburgh", Country = "USA" }] };
            var james = new Author { Name = "James Hess", Affiliations = [new() { Name = "Software Engineering Institute", City = "Pittsburgh", Country = "USA" }] };
            var william = new Author { Name = "William Nowak", Affiliations = [new() { Name = "Software Engineering Institute", City = "Pittsburgh", Country = "USA" }] };
            var spencer = new Author { Name = "Spencer Peterson", Affiliations = [new() { Name = "Software Engineering Institute", City = "Pittsburgh", Country = "USA" }] };
            var thomas = new Author { Name = "Thomas Thüm", Affiliations = [new() { Name = "University of Magdeburg", City = "Magdeburg", Country = "Germany" }] };
            var fabian = new Author { Name = "Fabian Benduhn", Affiliations = [new() { Name = "University of Magdeburg", City = "Magdeburg", Country = "Germany" }] };
            var jens = new Author { Name = "Jens Meinicke", Affiliations = [new() { Name = "University of Magdeburg", City = "Magdeburg", Country = "Germany" }] };
            var thomasL = new Author { Name = "Thomas Leich", Affiliations = [new() { Name = "Metop GmbH", City = "Magdeburg", Country = "Germany" }] };
            var jude = new Author { Name = "Jude Gyimah", Affiliations = [Chalmers()] };
            var kevin = new Author { Name = "Kevin Hermann", Affiliations = [Chalmers()] };
            var janPhilipp = new Author { Name = "Jan-Philipp Steghöfer", Affiliations = [Chalmers()] };
            var ahmad = new Author { Name = "Ahmad Al Shihabi", Affiliations = [Rub()] };
            var jan = new Author { Name = "Jan Sollmann", Affiliations = [Rub()] };
            var rafael = new Author { Name = "Rafael Capilla", Affiliations = [new() { Name = "Rey Juan Carlos University", City = "Madrid", Country = "Spain" }] };
            // &end[Authors]

            db.Papers.AddRange(
                new Paper
                {
                    Title = "HAnS: IDE-Based Editing Support for Embedded Feature Annotations",
                    Year = 2021,
                    Abstract = "Presents HAnS, an IntelliJ plugin for editing embedded feature annotations in source code, providing features such as code completion, navigation, and consistency checking.",
                    Doi = null,
                    Authors = [johan, herman, mukelabai, thorsten, alexandre, truong]
                },
                new Paper
                {
                    Title = "What Is a Feature? A Qualitative Study of Features in Industrial Software Product Lines",
                    Year = 2015,
                    Abstract = "Reports a qualitative study investigating what practitioners consider to be features in industrial software product lines across multiple companies.",
                    Doi = null,
                    Authors = [thorsten, daniela, julia, paul, adeline, martin, marsha, krzysztof]
                },
                new Paper
                {
                    Title = "Variability Mining with LEADT",
                    Year = 2011,
                    Abstract = "Presents LEADT, a tool for mining feature-to-code mappings from preprocessor-based software product lines.",
                    Doi = null,
                    Authors = [christian, alexander, klaus]
                },
                new Paper
                {
                    Title = "Feature-Oriented Software Product Lines: Concepts and Implementation",
                    Year = 2013,
                    Abstract = "A comprehensive textbook covering the theory and practice of feature-oriented software product lines, including feature modeling, variability implementation, and product derivation.",
                    Doi = "10.1007/978-3-642-37521-7",
                    Authors = [sven, don, christian, gunter]
                },
                new Paper
                {
                    Title = "Feature-Oriented Domain Analysis (FODA) Feasibility Study",
                    Year = 1990,
                    Abstract = "Introduces Feature-Oriented Domain Analysis, a method for identifying and organizing the features of software systems in a domain to support reuse.",
                    Doi = null,
                    Authors = [kyo, sholom, james, william, spencer]
                },
                new Paper
                {
                    Title = "FeatureIDE: An Extensible Framework for Feature-Oriented Software Development",
                    Year = 2014,
                    Abstract = "Presents FeatureIDE, an Eclipse-based framework that supports all phases of feature-oriented software development, from domain analysis to product derivation.",
                    Doi = null,
                    Authors = [thomas, christian, fabian, jens, gunter, thomasL]
                },
                new Paper
                {
                    Title = "FM-PRO: A Feature Modeling Process",
                    Year = 2025,
                    Abstract = "Presents FM-PRO, an empirically grounded process for feature modeling in software product lines, evaluated through a multi-year industrial case study.",
                    Doi = "10.1109/TSE.2024.3513635",
                    Authors = [johan, wardah, jude, thorsten]
                },
                new Paper
                {
                    Title = "FeatRacer: Locating Features Through Assisted Traceability",
                    Year = 2023,
                    Abstract = "Presents FeatRacer, an approach and tool for locating features in source code through assisted traceability, combining automated analysis with human guidance.",
                    Doi = "10.1109/TSE.2023.3290613",
                    Authors = [mukelabai, kevin, thorsten, janPhilipp]
                },
                // &begin[Groups]
                new Paper
                {
                    Title = "On Using LLMs to 'Featurize' Software",
                    Year = 2024,
                    Abstract = "Reports exploratory experiments on using large language models to featurize software; defines featurization precisely and determines the potential and limitations of current LLMs for this task.",
                    Doi = null,
                    Authors = [johan, thorsten]
                },
                new Paper
                {
                    Title = "An IDE Plugin for Clone Management in Software Product Lines",
                    Year = 2024,
                    Abstract = "Presents an extension of HAnS with clone management capabilities using a lightweight trace database to support synchronization of cloned assets across software product line variants.",
                    Doi = null,
                    Authors = [johan, ahmad, jan, wardah, thorsten]
                },
                new Paper
                {
                    Title = "Visualizing Feature-Oriented Software Evolution",
                    Year = 2025,
                    Abstract = "Presents a feature evolution timeline IntelliJ plugin that visualizes feature evolution via traceability links and commit history, lifting abstraction to the feature level; validated through three action research cycles and a controlled experiment.",
                    Doi = null,
                    Authors = [johan, thorsten]
                },
                new Paper
                {
                    Title = "Lightweight Visualization of Software Features with HAnS-viz",
                    Year = 2025,
                    Abstract = "Presents HAnS-viz, an IntelliJ plugin providing feature-oriented visualizations that build on HAnS embedded annotations to lift code-level assets to feature-level representations.",
                    Doi = null,
                    Authors = [johan, thorsten]
                },
                new Paper
                {
                    Title = "Towards Decentralized Feature Models",
                    Year = 2026,
                    Abstract = "Proposes a framework for decentralized feature modeling; studies core concerns including placement, scope, cross-model dependencies, reuse, and integration through a 15-year systematic literature review.",
                    Doi = null,
                    Authors = [johan, wardah, mukelabai, rafael, thorsten]
                }
                // &end[Groups]
            );

            await db.SaveChangesAsync();
        }

        // &begin[Groups]
        if (!await db.Groups.AnyAsync())
        {
            var acceptedTitles = new HashSet<string>
            {
                "HAnS: IDE-Based Editing Support for Embedded Feature Annotations",
                "On Using LLMs to 'Featurize' Software",
                "FM-PRO: A Feature Modeling Process",
                "An IDE Plugin for Clone Management in Software Product Lines",
                "Visualizing Feature-Oriented Software Evolution",
                "Lightweight Visualization of Software Features with HAnS-viz",
                "Towards Decentralized Feature Models"
            };

            var acceptedPapers = await db.Papers
                .Where(p => acceptedTitles.Contains(p.Title))
                .ToListAsync();

            db.Groups.Add(new Group
            {
                Name = "Accepted Papers",
                Description = "Johan Martinson's published and accepted papers across SPLC, TSE, AISoLA, and Variability venues.",
                Papers = acceptedPapers
            });

            await db.SaveChangesAsync();
        }
        // &end[Groups]
    }
}
