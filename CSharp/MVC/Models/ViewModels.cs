//Notes
  -ViewModels shouldn't know about the database, they are just there to use models in a flexible way and for validation.
  -Don't return ViewModels to the Controllers unless necessary

//Load multiple Models to ViewModels
public class IndexViewModel {
    public IEnumerable<Model1> model1 { get; set; }
    public IEnumerable<Model2> model2 { get; set; }
}
    //How to use:
public ActionResult Index() {
	var myviewmodel = new IndexViewModel();
	myviewmodel.model1 = db.model1.ToList();
	myviewmodel.model2 = db.model2.ToList();
	return View(myviewmodel);
}
		
//Validations with DataAnnotations (EF)
//using System.ComponentModel.DataAnnotations;
//using System.Web.Mvc;

[Display(Name = "Name:")]

[Required(ErrorMessage = "Error.")]

[MaxLength(30, ErrorMessage = "Error, max 30.")]

[MinLength(3, ErrorMessage = "Error, min 3.")]

[Remote("Action", "Controller", HttpMethod = "POST", ErrorMessage = "Error.")]

[DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]

[RegularExpression(pattern: "[0-9]{2}[0-9]{3}[0-9]{3}-[k-k0-9]{1}", ErrorMessage = "Error.")]

//Complete email regex xx@xx.xx
[RegularExpression(pattern: @"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$", ErrorMessage = "Error, invalid email.")]        




