namespace IrishPets.Models
{
    public class ContactInfoViewModel : Member
    {
        public ContactInfoViewModel() { }

        public ContactInfoViewModel(Member _member) 
        {
            this.FirstName = _member.FirstName;
            this.Surname = _member.Surname;
            this.Note = _member.Note;
            this.DateOfBirth = _member.DateOfBirth;
            this.Gender = _member.Gender;
            this.PhoneNumber = _member.PhoneNumber;
            this.Email = _member.Email;
            this.HouseNameNo = _member.HouseNameNo;
            this.Street = _member.Street;
            this.Town = _member.Town;
            this.County = _member.County;
            this.Postcode = _member.Postcode;
            this.DateOfLastLogin = _member.DateOfLastLogin;
        }

        public void Update(Member _member)
        {

            _member.FirstName = this.FirstName;
            _member.Surname = this.Surname;
            _member.Note = this.Note;
            _member.DateOfBirth = this.DateOfBirth;
            _member.Gender = this.Gender;
            _member.PhoneNumber = this.PhoneNumber;
            _member.HouseNameNo = this.HouseNameNo;
            _member.Street = this.Street;
            _member.Town = this.Town;
            _member.CountyId = this.CountyId;
            _member.Postcode = this.Postcode;
        }
    }
}