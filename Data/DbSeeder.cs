using Microsoft.EntityFrameworkCore;
using ReferenceManager.Models;

namespace ReferenceManager.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        if (await db.Papers.AnyAsync()) return;

        db.Papers.AddRange(
            new Paper
            {
                Title = "HAnS: IDE-Based Editing Support for Embedded Feature Annotations",
                Year = 2021,
                Abstract = "Presents HAnS, an IntelliJ plugin for editing embedded feature annotations in source code, providing features such as code completion, navigation, and consistency checking.",
                Doi = null,
                Authors =
                [
                    new() { Name = "Johan Martinson", Email = null, Affiliations = [new() { Name = "Chalmers University of Technology", City = "Gothenburg", Country = "Sweden" }] },
                    new() { Name = "Herman Jansson", Email = null, Affiliations = [new() { Name = "Chalmers University of Technology", City = "Gothenburg", Country = "Sweden" }] },
                    new() { Name = "Mukelabai Mukelabai", Email = null, Affiliations = [new() { Name = "Chalmers University of Technology", City = "Gothenburg", Country = "Sweden" }] },
                    new() { Name = "Thorsten Berger", Email = null, Affiliations = [new() { Name = "Chalmers University of Technology", City = "Gothenburg", Country = "Sweden" }] },
                    new() { Name = "Alexandre Bergel", Email = null, Affiliations = [new() { Name = "University of Chile", City = "Santiago", Country = "Chile" }] },
                    new() { Name = "Truong Ho-Quang", Email = null, Affiliations = [new() { Name = "Chalmers University of Technology", City = "Gothenburg", Country = "Sweden" }] },
                ]
            },
            new Paper
            {
                Title = "What Is a Feature? A Qualitative Study of Features in Industrial Software Product Lines",
                Year = 2015,
                Abstract = "Reports a qualitative study investigating what practitioners consider to be features in industrial software product lines across multiple companies.",
                Doi = null,
                Authors =
                [
                    new() { Name = "Thorsten Berger", Email = null, Affiliations = [new() { Name = "Chalmers University of Technology", City = "Gothenburg", Country = "Sweden" }] },
                    new() { Name = "Daniela Lettner", Email = null, Affiliations = [new() { Name = "Johannes Kepler University Linz", City = "Linz", Country = "Austria" }] },
                    new() { Name = "Julia Rubin", Email = null, Affiliations = [new() { Name = "University of British Columbia", City = "Vancouver", Country = "Canada" }] },
                    new() { Name = "Paul Grünbacher", Email = null, Affiliations = [new() { Name = "Johannes Kepler University Linz", City = "Linz", Country = "Austria" }] },
                    new() { Name = "Adeline Silva", Email = null, Affiliations = [] },
                    new() { Name = "Martin Becker", Email = null, Affiliations = [] },
                    new() { Name = "Marsha Chechik", Email = null, Affiliations = [new() { Name = "University of Toronto", City = "Toronto", Country = "Canada" }] },
                    new() { Name = "Krzysztof Czarnecki", Email = null, Affiliations = [new() { Name = "University of Waterloo", City = "Waterloo", Country = "Canada" }] },
                ]
            },
            new Paper
            {
                Title = "Variability Mining with LEADT",
                Year = 2011,
                Abstract = "Presents LEADT, a tool for mining feature-to-code mappings from preprocessor-based software product lines.",
                Doi = null,
                Authors =
                [
                    new() { Name = "Christian Kästner", Email = null, Affiliations = [new() { Name = "Philipps-Universität Marburg", City = "Marburg", Country = "Germany" }] },
                    new() { Name = "Alexander Dreiling", Email = null, Affiliations = [new() { Name = "Philipps-Universität Marburg", City = "Marburg", Country = "Germany" }] },
                    new() { Name = "Klaus Ostermann", Email = null, Affiliations = [new() { Name = "Philipps-Universität Marburg", City = "Marburg", Country = "Germany" }] },
                ]
            },
            new Paper
            {
                Title = "Feature-Oriented Software Product Lines: Concepts and Implementation",
                Year = 2013,
                Abstract = "A comprehensive textbook covering the theory and practice of feature-oriented software product lines, including feature modeling, variability implementation, and product derivation.",
                Doi = "10.1007/978-3-642-37521-7",
                Authors =
                [
                    new() { Name = "Sven Apel", Email = null, Affiliations = [new() { Name = "University of Passau", City = "Passau", Country = "Germany" }] },
                    new() { Name = "Don Batory", Email = null, Affiliations = [new() { Name = "University of Texas at Austin", City = "Austin", Country = "USA" }] },
                    new() { Name = "Christian Kästner", Email = null, Affiliations = [new() { Name = "Carnegie Mellon University", City = "Pittsburgh", Country = "USA" }] },
                    new() { Name = "Gunter Saake", Email = null, Affiliations = [new() { Name = "University of Magdeburg", City = "Magdeburg", Country = "Germany" }] },
                ]
            },
            new Paper
            {
                Title = "Feature-Oriented Domain Analysis (FODA) Feasibility Study",
                Year = 1990,
                Abstract = "Introduces Feature-Oriented Domain Analysis, a method for identifying and organizing the features of software systems in a domain to support reuse.",
                Doi = null,
                Authors =
                [
                    new() { Name = "Kyo Kang", Email = null, Affiliations = [new() { Name = "Software Engineering Institute", City = "Pittsburgh", Country = "USA" }, new() { Name = "Carnegie Mellon University", City = "Pittsburgh", Country = "USA" }] },
                    new() { Name = "Sholom Cohen", Email = null, Affiliations = [new() { Name = "Software Engineering Institute", City = "Pittsburgh", Country = "USA" }] },
                    new() { Name = "James Hess", Email = null, Affiliations = [new() { Name = "Software Engineering Institute", City = "Pittsburgh", Country = "USA" }] },
                    new() { Name = "William Nowak", Email = null, Affiliations = [new() { Name = "Software Engineering Institute", City = "Pittsburgh", Country = "USA" }] },
                    new() { Name = "Spencer Peterson", Email = null, Affiliations = [new() { Name = "Software Engineering Institute", City = "Pittsburgh", Country = "USA" }] },
                ]
            },
            new Paper
            {
                Title = "FeatureIDE: An Extensible Framework for Feature-Oriented Software Development",
                Year = 2014,
                Abstract = "Presents FeatureIDE, an Eclipse-based framework that supports all phases of feature-oriented software development, from domain analysis to product derivation.",
                Doi = null,
                Authors =
                [
                    new() { Name = "Thomas Thüm", Email = null, Affiliations = [new() { Name = "University of Magdeburg", City = "Magdeburg", Country = "Germany" }] },
                    new() { Name = "Christian Kästner", Email = null, Affiliations = [new() { Name = "Carnegie Mellon University", City = "Pittsburgh", Country = "USA" }] },
                    new() { Name = "Fabian Benduhn", Email = null, Affiliations = [new() { Name = "University of Magdeburg", City = "Magdeburg", Country = "Germany" }] },
                    new() { Name = "Jens Meinicke", Email = null, Affiliations = [new() { Name = "University of Magdeburg", City = "Magdeburg", Country = "Germany" }] },
                    new() { Name = "Gunter Saake", Email = null, Affiliations = [new() { Name = "University of Magdeburg", City = "Magdeburg", Country = "Germany" }] },
                    new() { Name = "Thomas Leich", Email = null, Affiliations = [new() { Name = "Metop GmbH", City = "Magdeburg", Country = "Germany" }] },
                ]
            },
            new Paper
            {
                Title = "FM-PRO: A Feature Modeling Process",
                Year = 2025,
                Abstract = "Presents FM-PRO, an empirically grounded process for feature modeling in software product lines, evaluated through a multi-year industrial case study.",
                Doi = "10.1109/TSE.2024.3513635",
                Authors =
                [
                    new() { Name = "Johan Martinson", Email = null, Affiliations = [new() { Name = "Chalmers University of Technology", City = "Gothenburg", Country = "Sweden" }] },
                    new() { Name = "Wardah Mahmood", Email = null, Affiliations = [new() { Name = "Chalmers University of Technology", City = "Gothenburg", Country = "Sweden" }] },
                    new() { Name = "Jude Gyimah", Email = null, Affiliations = [new() { Name = "Chalmers University of Technology", City = "Gothenburg", Country = "Sweden" }] },
                    new() { Name = "Thorsten Berger", Email = null, Affiliations = [new() { Name = "Ruhr-Universität Bochum", City = "Bochum", Country = "Germany" }] },
                ]
            },
            new Paper
            {
                Title = "FeatRacer: Locating Features Through Assisted Traceability",
                Year = 2023,
                Abstract = "Presents FeatRacer, an approach and tool for locating features in source code through assisted traceability, combining automated analysis with human guidance.",
                Doi = "10.1109/TSE.2023.3290613",
                Authors =
                [
                    new() { Name = "Mukelabai Mukelabai", Email = null, Affiliations = [new() { Name = "Chalmers University of Technology", City = "Gothenburg", Country = "Sweden" }] },
                    new() { Name = "Kevin Hermann", Email = null, Affiliations = [new() { Name = "Chalmers University of Technology", City = "Gothenburg", Country = "Sweden" }] },
                    new() { Name = "Thorsten Berger", Email = null, Affiliations = [new() { Name = "Chalmers University of Technology", City = "Gothenburg", Country = "Sweden" }] },
                    new() { Name = "Jan-Philipp Steghöfer", Email = null, Affiliations = [new() { Name = "Chalmers University of Technology", City = "Gothenburg", Country = "Sweden" }] },
                ]
            }
        );

        await db.SaveChangesAsync();
    }
}
