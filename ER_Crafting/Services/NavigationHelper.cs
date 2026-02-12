using Microsoft.AspNetCore.Components;

namespace ER_Crafting.Services
{
    public class NavigationHelper
    {
        private readonly NavigationManager _navigationManager;

        public NavigationHelper(NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
        }

        /// <summary>
        /// Navigate to a page with proper base path handling for GitHub Pages
        /// </summary>
        /// <param name="relativePath">Path relative to app root (e.g. "finalitems", "gear", "colonies")</param>
        /// <param name="queryParams">Optional query parameters</param>
        public void NavigateToPage(string relativePath, Dictionary<string, string>? queryParams = null)
        {
            // Remove leading slash if present
            relativePath = relativePath.TrimStart('/');

            // Get the proper base URI from NavigationManager
            var baseUri = new Uri(_navigationManager.BaseUri);
            var correctPath = baseUri.AbsolutePath.TrimEnd('/') + "/" + relativePath;

            // Handle GitHub Pages case where base path might not be set correctly
            var currentUri = new Uri(_navigationManager.Uri);
            if (currentUri.Host.Contains("github.io"))
            {
                // Ensure we have the repository name in the path for GitHub Pages
                if (!correctPath.Contains("/ER_Crafting/") && baseUri.AbsolutePath == "/")
                {
                    correctPath = "/ER_Crafting/" + relativePath;
                }
            }

            string finalUrl;
            if (queryParams != null && queryParams.Count > 0)
            {
                finalUrl = Microsoft.AspNetCore.WebUtilities.QueryHelpers.AddQueryString(correctPath, queryParams);
            }
            else
            {
                finalUrl = correctPath;
            }

            _navigationManager.NavigateTo(finalUrl);
        }

        /// <summary>
        /// Get the correct URL for a page (useful for href attributes)
        /// </summary>
        /// <param name="relativePath">Path relative to app root</param>
        /// <returns>Correct URL with proper base path</returns>
        public string GetPageUrl(string relativePath)
        {
            relativePath = relativePath.TrimStart('/');
            var baseUri = new Uri(_navigationManager.BaseUri);
            var correctPath = baseUri.AbsolutePath.TrimEnd('/') + "/" + relativePath;

            var currentUri = new Uri(_navigationManager.Uri);
            if (currentUri.Host.Contains("github.io"))
            {
                if (!correctPath.Contains("/ER_Crafting/") && baseUri.AbsolutePath == "/")
                {
                    correctPath = "/ER_Crafting/" + relativePath;
                }
            }

            return correctPath;
        }
        public void NavigateToFinalItems(string? filter = null, bool? female = null, string? name = null)
        {
            var queryParams = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(filter))
                queryParams["filter"] = filter;

            if (!string.IsNullOrEmpty(name))
                queryParams["name"] = name;

            if (female.HasValue && female.Value)
                queryParams["female"] = "true";

            NavigateToPage("finalitems", queryParams);
        }
    }
}