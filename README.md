# Gift of the Givers – ASP.NET Core Prototype

## Requirements covered
- Gift of the Givers branding/logo and header
- Responsive Bootstrap navigation: Home, About, Donate, Volunteer, Contact
- ASP.NET Core 8 Razor Pages
- ASP.NET Core Identity with Employee and Donor roles
- Employee login and protected employee dashboard
- Donor login support
- Anonymous donation option
- One-time or recurring donations
- ZAR, USD and EUR symbolic currencies
- Placeholder tax certificate after every donation, printable / saveable as PDF
- Volunteer form: name, skills and availability
- Volunteer submissions stored in memory
- Employee can post ongoing relief-project updates
- Employee can view volunteer sign-ups and donation counts
- Prototype intentionally uses in-memory data for donation/volunteer/update records

## Run
1. Install .NET 8 SDK.
2. Open a terminal in this folder.
3. Run `dotnet restore`.
4. Run `dotnet run`.
5. Open the HTTPS URL shown by .NET.

### Demo employee account
Email: employee@givers.org
Password: Password1

The Identity database uses EF Core InMemory, so prototype account data is reset when the application restarts.
