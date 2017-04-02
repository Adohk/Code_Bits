//Notes
	-Can use CustomAttribues [CustomAttributeName] before every action or before the controller to make it global.
	-ModelState.IsValid checks if the info complies with the given model in the action.
 
//Attributes
[AllowAnonymous]
[HttpPost]
[HttpPost, ActionName("ActionName")] //A post from a view of the same name
[ValidateAntiForgeryToken] //Token for forms in views
[OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")] //No Cache
  
//EF db context.
private MyDBEntities db = new MyDBEntities();

//db context dispose.
protected override void Dispose(bool disposing) {
	if (disposing) {
		db.Dispose();
	}
	base.Dispose(disposing);
}

//List to view.
public ActionResult Index() {
    return View(db.model1.ToList());
}

//List single from id.
public ActionResult idList(int? id) {
	if (id == null) {
		return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
	}
	Model1 thismodel = db.Model1.Find(id);
	if (thismodel == null) {
		return HttpNotFound();
	}
	return View(thismodel);
}

//Post from view of some name & Delete/Edit.
[HttpPost, ActionName("Name")]
[ValidateAntiForgeryToken]
public ActionResult NameConfirm(int id) {
	Model1 thismodel = db.Model1.Find(id);
	//Delete
	//db.Model1.Remove(thismodel);
	
	//Edit
	thismodel.FieldName = "somethingnew";
	db.Entry(thismodel).State = EntityState.Modified;
	db.SaveChanges();	
	return RedirectToAction("Index");
}

//Insert to table.
[HttpPost]
[ValidateAntiForgeryToken]
public ActionResult Create([Bind(Include = "Id,Name")] Model model) {
	if (ModelState.IsValid) {
		db.Model.Add(model);
		db.SaveChanges();
		return RedirectToAction("Index");
	}
	return View(model);
}


//Json Result for Remote validations.
public JsonResult ActionNameFromModelRemoteAnnotation(string idnum) {
	var user = db.Model1.FirstOrDefault(p => p.ID == idnum);
	return Json(user == null); //Returns false if user is null
}

//Return a file to user.
public FileResult DownloadExcel() {
	string path = "/Format/Excel.xlsx";
	return File(path, "application/vnd.ms-excel", "Excel.xlsx");
}

//JavaScriptResult, check Uploaded File using LinqToExcel and mapping viewmodel to model using AutoMapper.

public JavaScriptResult UploadExcel(HttpPostedFileBase FileUpload) {
    StringBuilder data = new StringBuilder();
    StringBuilder errors = new StringBuilder();
    if (FileUpload != null) {
        if (FileUpload.ContentType == "application/vnd.ms-excel" || FileUpload.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet") {

            string pathToExcelFile = (Server.MapPath("~/Doc/")) + (FileUpload.FileName);
            FileUpload.SaveAs(pathToExcelFile);
			
			//LinqToExcel Instance
            var excel = new ExcelQueryFactory(pathToExcelFile);
            var sheet = excel.GetWorksheetNames();

            var contents = from c in excel.Worksheet<UsuariosValidationViewModel>(sheet.First()) select c;
            int counter = 0, row = 2;
            errors.AppendLine();
            foreach (var a in contents) {
                if (row > 100) { break; }
                row++;
                /* Validations */
		try {                                
			SomeValidationViewModel u = a
			ModelState.Clear();
			//Manually validate model (Throws exception) otherwise "TryValidateModel" returns a boolean.
			ValidateModel(u);

			//Mapping viewmodel to model using AutoMapper
			var config = new MapperConfiguration(cfg => {
				cfg.CreateMap<SomeValidationViewModel, Model1>();
			});
			IMapper mapper = config.CreateMapper();
			var users = mapper.Map<SomeValidationViewModel, Model1>(u);

			db.Model1.Add(users);
			db.SaveChanges();
		} catch (Exception) {
			db.Dispose();
			//Check all errors in ModelState
			var query = from state in ModelState.Values
						from error in state.Errors
						select error.ErrorMessage;
			var er = query.ToArray();
			errors.AppendLine("User: " + a.ID + " With Errors in Row: " + row + " Cause: " + er.FirstOrDefault() + ".");
			counter++;
		}                                        
            }
            //Deleting excel file from folder  
            if ((System.IO.File.Exists(pathToExcelFile))) {
                System.IO.File.Delete(pathToExcelFile);
            }
			//Checking if file is valid or if it doesn't have any content.
            if (!contents.Any()) {
                return JavaScript("Invalid or Empty File");
            }

            if (counter <= 0) {
                data.AppendLine("No errors!");
            } else {
                data.AppendLine(counter + " Rows with error, please check your file." + Environment.NewLine + errors);
            }
            return JavaScript(data.ToString());
        } else {
            return JavaScript("Only Excel files permitted.");
        }
    } else {
        return JavaScript("Please select a file first.");
    }
}

//Feeding viewbag to partialview with dropdownlists.
[HttpPost]
[OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
public PartialViewResult Create_logic(string selection) {
	List<SelectListItem> users = new List<SelectListItem>();
	List<SelectListItem> computers = new List<SelectListItem>();		      

	foreach (var usr in db.Model1) {
		if (usr.State == "Active") 
			users.Add(new SelectListItem { Text = usr.Name, Value = usr.Id });		
	}
	foreach (var cmp in db.INV_PC) {
		if (cmp.state == "Active") 
			computers.Add(new SelectListItem { Text = cmp.Code, Value = cmp.Id });		
	}
	ViewBag.Users = users;
	ViewBag.Computers = computers;

	return PartialView("_MyPartialView");
}
