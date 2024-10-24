using Microsoft.AspNetCore.Mvc;
using ClaimApp.Data;
using ClaimApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using ClaimApp.Hubs;

public class ClaimsController : Controller
{
    private readonly ClaimAppContext _context;
    private readonly IHubContext<ClaimStatusHub> _hubContext;

    public ClaimsController(ClaimAppContext context, IHubContext<ClaimStatusHub> hubContext)
    {
        _context = context;
        _hubContext = hubContext;
    }

    // Method to approve a claim
    public async Task<IActionResult> ApproveClaim(int claimId)
    {
        // Find the claim in the database by its ID
        var claim = await _context.Claims.FirstOrDefaultAsync(c => c.Id == claimId);

        if (claim == null)
        {
            return NotFound(); // Return a 404 if the claim doesn't exist
        }

        // Update the claim status to "Approved"
        claim.Status = "Approved";

        // Save the changes to the database
        _context.Claims.Update(claim);
        await _context.SaveChangesAsync();

        // Notify all connected clients about the status update
        await _hubContext.Clients.All.SendAsync("ReceiveClaimStatusUpdate", claimId, "Approved");

        // Redirect to the Index action (or whichever page you want to return to)
        return RedirectToAction("Index");
    }

    // Method to reject a claim
    public async Task<IActionResult> RejectClaim(int claimId)
    {
        // Find the claim in the database by its ID
        var claim = await _context.Claims.FirstOrDefaultAsync(c => c.Id == claimId);

        if (claim == null)
        {
            return NotFound(); // Return a 404 if the claim doesn't exist
        }

        // Update the claim status to "Rejected"
        claim.Status = "Rejected";

        // Save the changes to the database
        _context.Claims.Update(claim);
        await _context.SaveChangesAsync();

        // Notify all connected clients about the status update
        await _hubContext.Clients.All.SendAsync("ReceiveClaimStatusUpdate", claimId, "Rejected");

        // Redirect to the Index action (or whichever page you want to return to)
        return RedirectToAction("Index");
    }
}
