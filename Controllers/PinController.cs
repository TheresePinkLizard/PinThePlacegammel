using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PinThePlace.Models;
using PinThePlace.ViewModels;


namespace PinThePlace.Controllers;

public class PinController : Controller
{

    private readonly PinDbContext _pinDbContext; // deklarerer en privat kun lesbar felt for å lagre instanser av ItemDbContext

    public PinController(PinDbContext pinDbContext) // konstruktør som tar en ItemDbContext instans som et parameter og assigner til _itemDbContext 
    {                                                           // Dette er et eksempel på en dependency injectionm hvor DbContext is provided to the controllerer via ASP.NET Core rammeverk.
        _pinDbContext = pinDbContext;                         //Konstruktøren blir kalt når en instans er laget, vanligvis under behandling av inkommende HTTP request. Når Views er kalt. eks: table grid, details
    }

    // async i metodene:
    // gjør siden mer responsive. den lar programmet kjøre flere tasks concurrently uten å blokkere main thread.
    // dette får siden til å virke mer responsiv ved å la andre oppgaver gå i forveien istedet for at alt venter på et program.
    // await hører også til async

    // en action som korresponderer til en brukers interaksjon, slik som å liste opp items når en url lastes
    public async Task<IActionResult> Table()
    {  
        // henter alle items fra items table i databasen og konverterer til en liste
        List<Pin> pins = await _pinDbContext.Pins.ToListAsync();

        var pinsViewModel = new PinsViewModel(pins, "Table");
        // en action kan returnere enten: View, JSON, en Redirect, eller annet. 
        // denne returnerer en view
        return View(pinsViewModel);
    }

    public async Task<IActionResult> Comments(int id)
    {
        List<Pin> pins = await _pinDbContext.Pins.ToListAsync();
        var pin= pins.FirstOrDefault(i => i.PinId == id); // søker igjennom listen items til første som matcher id
        if (pin == null)
            return NotFound();
        return View(pin); // returnerer view med et item
    }

    //  Http Get og post for å gjøre CRUD
    //Get: It returns a view (the "Create" view) that contains a form where the user can enter details for creating the new item
    [HttpGet]
    public IActionResult Create() // trigges når bruker navigerer til create siden
    {
        return View(); // returnerer view hvor bruker kan skrice inn detaljer for å lage et nytt item
    }

// post:  is used to handle the submission of the form when the user clicks the "Create" button
    [HttpPost]
    public async Task<IActionResult> Create(Pin pin) // tar inn item objekt som parameter
    {
        if (ModelState.IsValid) // sjekker validering
        {
            _pinDbContext.Pins.Add(pin);  //legges til i database
           await _pinDbContext.SaveChangesAsync(); // endringer lagres
            return RedirectToAction(nameof(Table)); // redirects to show items in table
        }
        return View(pin);
    }

    // kodene under gjør at update og delete fungerer
    [HttpGet]
    public async Task<IActionResult> Update(int id)  // denne metoden viser utfyllingsskjemaet for å oppdatere en eksisterende item
    {                                   // metoden slår ut når bruker navigerer seg til update siden
        var pin = await _pinDbContext.Pins.FindAsync(id); // henter fra database ved hjelp av id
        if (pin == null)               // sjekk om den finner item
        {
            return NotFound();
        }
        return View(pin); 
    }

    [HttpPost]
    public async Task<IActionResult> Update(Pin pin)  // tar informasjonen som er skrevet i update skjema,
    {                                           // ser hvis det er valid og oppdaterer i database
        if (ModelState.IsValid)
        {
            _pinDbContext.Pins.Update(pin);
            await _pinDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Table)); // displayer den oppdaterte listen
        }
        return View(pin);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)  // displayer confirmation page for å slette en item
    {
        var pin = await _pinDbContext.Pins.FindAsync(id);  // identifiserer og henter item som skal bli slettet
        if (pin == null)
        {
            return NotFound();
        }
        return View(pin);   // hvis funnet, returnerer view med item data for bekreftelse
    }

    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(int id) // metoden som faktisk sletter item fra database
    {
        var pin= await _pinDbContext.Pins.FindAsync(id); // finner item i database ved bruk at id
        if (pin == null)
        {
            return NotFound();
        }
        _pinDbContext.Pins.Remove(pin); // sletter item
        await _pinDbContext.SaveChangesAsync();  // lagrer endringene 
        return RedirectToAction(nameof(Table)); //returnerer bruker til table view hvor item nå er fjernet
    }
    
}

