// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Microsoft.AspNet.Authentication.JwtBearer
{
    /// <summary>
    /// Options class provides information needed to control Bearer Authentication middleware behavior
    /// </summary>
    public class JwtBearerAuthenticationOptions : AuthenticationOptions
    {
        /// <summary>
        /// Creates an instance of bearer authentication options with default values.
        /// </summary>
        public JwtBearerAuthenticationOptions() : base()
        {
            AuthenticationScheme = JwtBearerAuthenticationDefaults.AuthenticationScheme;
        }

        /// <summary>
        /// Gets or sets the discovery endpoint for obtaining metadata
        /// </summary>
        public string MetadataAddress { get; set; }

        /// <summary>
        /// Gets or sets the Authority to use when making OpenIdConnect calls.
        /// </summary>
        public string Authority { get; set; }

        /// <summary>
        /// Gets or sets the audience for any received JWT token.
        /// </summary>
        /// <value>
        /// The expected audience for any received JWT token.
        /// </value>
        public string Audience { get; set; }

        /// <summary>
        /// Gets or sets the challenge to put in the "WWW-Authenticate" header.
        /// </summary>
        public string Challenge { get; set; } = JwtBearerAuthenticationDefaults.AuthenticationScheme;

        /// <summary>
        /// The object provided by the application to process events raised by the bearer authentication middleware.
        /// The application may implement the interface fully, or it may create an instance of JwtBearerAuthenticationProvider
        /// and assign delegates only to the events it wants to process.
        /// </summary>
        public JwtBearerAuthenticationNotifications Notifications { get; set; } = new JwtBearerAuthenticationNotifications();

        /// <summary>
        /// The HttpMessageHandler used to retrieve metadata.
        /// This cannot be set at the same time as BackchannelCertificateValidator unless the value
        /// is a WebRequestHandler.
        /// </summary>
        public HttpMessageHandler BackchannelHttpHandler { get; set; }

        /// <summary>
        /// Gets or sets the timeout when using the backchannel to make an http call.
        /// </summary>
        public TimeSpan BackchannelTimeout { get; set; } = TimeSpan.FromMinutes(1);

#if DNX451
        /// <summary>
        /// Gets or sets the a pinned certificate validator to use to validate the endpoints used
        /// when retrieving metadata.
        /// </summary>
        /// <value>
        /// The pinned certificate validator.
        /// </value>
        /// <remarks>If this property is null then the default certificate checks are performed,
        /// validating the subject name and if the signing chain is a trusted party.</remarks>
        public ICertificateValidator BackchannelCertificateValidator { get; set; }
#endif
        /// <summary>
        /// Configuration provided directly by the developer. If provided, then MetadataAddress and the Backchannel properties
        /// will not be used. This information should not be updated during request processing.
        /// </summary>
        public OpenIdConnectConfiguration Configuration { get; set; }

        /// <summary>
        /// Responsible for retrieving, caching, and refreshing the configuration from metadata.
        /// If not provided, then one will be created using the MetadataAddress and Backchannel properties.
        /// </summary>
        public IConfigurationManager<OpenIdConnectConfiguration> ConfigurationManager { get; set; }

        /// <summary>
        /// Gets or sets if a metadata refresh should be attempted after a SecurityTokenSignatureKeyNotFoundException. This allows for automatic
        /// recovery in the event of a signature key rollover. This is enabled by default.
        /// </summary>
        public bool RefreshOnIssuerKeyNotFound { get; set; } = true;

        /// <summary>
        /// Used to know what the current clock time is when calculating or validating token expiration. When not assigned default is based on
        /// DateTimeOffset.UtcNow. This is typically needed only for unit testing.
        /// </summary>
        public ISystemClock SystemClock { get; set; } = new SystemClock();

        /// <summary>
        /// Gets the ordered list of <see cref="ISecurityTokenValidator"/> used to validate access tokens.
        /// </summary>
        public IList<ISecurityTokenValidator> SecurityTokenValidators { get; } = new List<ISecurityTokenValidator> { new JwtSecurityTokenHandler() };

        /// <summary>
        /// Gets or sets the parameters used to validate identity tokens.
        /// </summary>
        /// <remarks>Contains the types and definitions required for validating a token.</remarks>
        /// <exception cref="ArgumentNullException">if 'value' is null.</exception>
        public TokenValidationParameters TokenValidationParameters { get; set; } = new TokenValidationParameters();
    }
}