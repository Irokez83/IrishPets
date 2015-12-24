using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace IrishPets.Migrations
{
    using Models;

    public static class ConfigurationEx
    {
        public static void CreateNewCounties(IrishPetsDb _context)
        {
            if (null != _context.Counties.FirstOrDefault())
            {
                return;
            }

            int __idx = 0;

            string __counties = @"Antrim,Armagh,Carlow,Cavan,Clare,Cork,Donegal,Down,Dublin,Fermanagh,Galway,Kerry,Kildare,Kilkenny,Laois,Leitrim,Limerick,Londonderry,Longford,Louth,Mayo,Meath,Monaghan,Offaly,Roscommon,Sligo,Tipperary,Tyrone,Waterford,Westmeath,Wexford,Wicklow";
            __counties
                .Split(',')
                .ToList()
                .ForEach(zzz =>
            {
                var __item = new County { Id = __idx++, Name = zzz };
                _context.Counties.Add(__item);
            });

            _context.SaveChanges();
        }

        //Used in creation of new DB. Assigns the specific role to the user.
        public static Member NewUser(ApplicationUserManager _manager, string _username, string _role = null)
        {
            var __user = new Member
            {
                Email = $"{_username}@{Properties.Resources.DefDomain}"
                ,
                UserName = $"{_username}@{Properties.Resources.DefDomain}"
                ,
                Gender = Gender.Male
                ,
                DateOfBirth = DateTime.Now.AddYears(-(int.Parse(DateTime.Now.AddMilliseconds(-30).ToString("ff"))))
                ,
                FirstName = $"{_username}-Name"
                ,
                Surname = $"{_username}-Surname"
                ,
                Postcode = "Dublin 24"
                ,
                HouseNameNo = $"{DateTime.Now.AddMilliseconds(-66).ToString("ff")}"
                ,
                Street = "Hopkins Road"
                ,
                Town = "Tallaght"
                ,
                CountyId = 9 // Dublin

                ,
                PhoneNumber = $"+353 (2) {DateTime.Now.AddMilliseconds(55).ToString("fff")}-{DateTime.Now.AddMilliseconds(66).ToString("ff")}-{DateTime.Now.AddMilliseconds(56).ToString("ff")}-{DateTime.Now.AddMilliseconds(-65).ToString("ff")}"
                ,
                Note = _username

                ,
                EmailConfirmed = true //e-mail confirmed
                ,
                DateOfLastLogin = DateTime.Now
            };

            var __result = _manager.Create(__user, $"#{_username}"); //Default password for the users : [#UserName]

            //If creation successfull then adding the roles
            if (__result.Succeeded)
            {
                //"User" role added by default
                _manager.AddToRole(__user.Id, "User");

                if (!string.IsNullOrEmpty(_role))
                {   //In case of "Admin" 
                    _manager.AddToRole(__user.Id, _role);
                }
            }

            return __user;
        }

        //Creates "Admin" and "User" at the time of DB creation
        public static void CreateNewUsersAndRole(IrishPetsDb _context)
        {
            var __roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_context));

            //Roles created
            var __roleAdmin = new IdentityRole { Name = "Admin" };
            var __roleUser = new IdentityRole { Name = "User" };

            //Roles added to DB
            __roleManager.Create(__roleAdmin);
            __roleManager.Create(__roleUser);

            var __userManager = new ApplicationUserManager(new UserStore<Member>(_context));

            //"Admin" creation - needed all the time
            NewUser(__userManager, "Admin001", "Admin");

            //Default "Users" creation
            NewUser(__userManager, "User001");
            NewUser(__userManager, "User002");
            NewUser(__userManager, "User003");
            NewUser(__userManager, "User004");
            NewUser(__userManager, "User005");
        }

        public static void CreateNewPetKind(IrishPetsDb _context)
        {
            if (null != _context.PetKinds.FirstOrDefault())
            {
                return;
            }

            int __idx = 0;

            @"Cat,Dog".Split(',').ToList().ForEach(zzz =>
            {
                var __item = new PetKind { Id = __idx++, Name = zzz, Breeds = new List<PetBreed>() };
                __item = _context.PetKinds.Add(__item);

                if (1 == __idx % 2)
                {
                    CCat().ForEach(nnn =>
                    {
                        _context.PetBreeds.Add(new PetBreed
                        {
                            Name = nnn,
                            Kind = __item,
                            KindId = __item.Id
                        });
                    });
                }
                else
                {
                    CDog().ForEach(nnn =>
                    {
                        _context.PetBreeds.Add(new PetBreed
                        {
                            Name = nnn,
                            Kind = __item,
                            KindId = __item.Id
                        });
                    });
                }
                _context.SaveChanges();
            });
        }

        private static List<string> CCat()
        {
            return @"Abyssinian
,Alaskan Kleekai
,American (Sh/Wireh)
,American Bobtail
,American Curl
,Angora
,Arabian Mau
,Asian Black Ticked Tabby
,Asian Red Ticked Tabby
,Asian Shorthair
,Australian Mist
,Balinese
,Bengal
,Berger De Picard
,Bi-Colour
,Birman
,Black Russian
,Blue Asian
,Blue Burmese
,Blue-Pointed Siamese
,BoerBoel
,Bombay
,British Blue
,British Cream
,British Exotic
,British Longhair
,British Red
,British Shorthair
,British Tip
,Brown Burmese
,Brown Tabby
,Burmese
,Burmese Cross
,Burmilla
,Chartreux
,Cheetoh
,Chinchilla
,Chinchilla Persian
,Chocolate Burmese
,Chocolate Tabby Point Siamese
,Chocolate-Pointed Siamese
,Colourpoint
,Cornish Rex
,Cream Burmese
,Cream-Pointed Siamese
,Crossbreed
,Desert Lynx
,Devon Rex
,Dogo Canario
,Domestic Long-Haired
,Domestic Medium Hair
,Domestic Shorthair
,Don Sphynx
,Dwarf Lop
,Egyptian Mau
,European Burmese
,Exotic
,Feral
,Foreign
,Foreign Blue
,Foreign Lilac
,Foreign Longhaired
,Foreign shorthair
,Foreign White
,French Lop
,Ginger
,Golden Tabby
,Gypsy Vanner
,Havana
,Himalayan
,Japanese Bobtail (Sh/Lh)
,Javanese
,Khao Manee
,Korat
,Kurilian Bobtail
,LaPerm
,Lilac Burmese
,Lilac Tortie Burmese
,Lilac-Pointed Siamese
,Maine Coon
,Manx
,Miniature
,Munchkin
,Nebelung
,Norwegian Forest Cat
,Ocicat
,Ocicat Classic
,Oriental Caramel Ticked Tabby
,Oriental Choc. Ticked Tabby
,Oriental Longhair
,Oriental Shorthair
,Persian
,Peterbald
,Pixie Bob
,RagaMuffin
,Ragdoll
,Red Burmese
,Red-Pointed Siamese
,Russian Black
,Russian Blue
,Russian Tabby
,Russian White
,Savannah
,Scottish Fold (Sh/Lh)
,Seal-Pointed Siamese
,Selkirk Rex
,Serengeti
,Show Cob
,Siamese
,Siamese Cross
,Siberian
,Silver Spotted Tabby
,Silver Tabby
,Singapura
,Snowshoe
,Sokoke
,Somali
,Sphynx
,Spotted Mist
,Tabby
,Tabby-Pointed Siamese
,Thai Blue Points
,Thai Lilacs
,Tibetan
,Tiffanie
,Tonkinese
,Tortie-Pointed Siamese
,Tortoiseshell
,Tortoiseshell And White
,Toyger
,Turkish Angora
,Turkish Van".Split(',').ToList();
        }

        private static List<string> CDog()
        {
            return @"Aberdeen Terrier
,Abyssinian Sand Terrier
,Affenpinscher
,Afghan Hound
,Africanis
,Aidi (Atlas Sheepdog)
,Airedale Terrier
,Akbash
,Akita
,Alaskan Kleekai
,Alaskan Malador
,Alaskan Malamute
,Alaskan Shepherd
,Alpha Blue Bulldog
,Alsatian
,Alusky
,American Bulldog
,American Cocker Spaniel
,American English Coonhound
,American Hairless Terrier
,American Indian Dog
,American Spitz
,American Water Spaniel
,Anatolian Karabash Dog
,Anatolian Shepherd Dog
,Appenzeller Sennenhund
,Argente
,Australian Cattle Dog
,Australian Kelpie Sheepdog
,Australian Shepherd Dog
,Australian Silky Terrier
,Australian Terrier
,Aylestone Old Tyme Bulldog
,Azawakh
,Bagel Hound
,Barbet
,Basenji
,Basser Bleu De Gascogne
,Basset Fauve De Bretagne
,Basset Griffon Vendeen
,Basset Hound
,Bavarian Mountain Hound
,Beagle
,Beaglier
,Bearded Collie
,Beaucheron
,Bedlington Terrier
,Belgian Laekenois Shepherd Dog
,Belgian Shepherd Dog
,Bergamasco Shepherd Dog
,Berger De Picard
,Bernese Mountain Dog
,Beveren
,Bich-Poo
,Bichon Frise
,Black and Tan Coonhound
,Black Labrador
,Black Mouth Cur
,Black Russian
,Bloodhound
,Blue Merle Welsh Collie
,luetick Coonhound
,BoerBoel
,Bolognese
,Borador
,Border Collie
,Border Springer
,Border Terrier
,Borzoi
,Boston Terrier
,Bouvier Des Flandres
,Boxer
,Bracco
,Braque d'Auvergne
,Briard
,British Timber dog
,Brittany Spaniel
,Bugg
,Bull Boxer Staffy Bull
,Bull Terrier
,Bull-Pei
,Bulldog
,Bullmasador
,Bullmastiff
,Caine Corso (Italian Mastiff)
,Cairn Terrier
,Cairnoodle
,Canaan
,Canadian Eskimo Dog
,Canarian Warren Hound
,Capheaton Terrier
,Cardigan Corgi
,Carpathian Shepherd Dog
,Catahoula Leopard Dog
,Catalan Sheepdog
,Cavachon
,Cavador
,Cavalier King Charles Spaniel
,Cavapoo
,Cesky Terrier
,Chesapeake Bay Retriever
,Chi Apso
,Chi-Poo
,Chihuahua
,Chinese Crested
,Chinese Crested Hairless
,Chinese Crested Powder Puff
,Chinese Crestepoo
,Chocolate Labrador
,Chorkie
,Chow Chow
,Chug
,Cirneco dell'Etna
,Clumber Spaniel
,Cockalier
,Cockapoo
,Cocker Jack
,Cocker Spaniel
,Comtois
,Continental Landseer
,Corgi
,Coton De Tulear
,Crossbreed
,Curly Coated Retriever
,Dachshund
,Dalmador
,Dalmatian
,Dandie Dinmont Terrier
,Danish Swedish Farm Dog
,Deerhound
,Dobermann
,Dobermann Pinscher
,Dogue de Bordeaux
,Dorgie
,Dorset Old Tyme Bulldog
,Dutch Partridge Dog
,Dutch Sheepdog
,Dutch Shepherd Dog
,Elkhound
,English Coonhound
,English Pointer
,English Setter
,English Shepherd
,English Springer Spaniel
,English Toy Terrier
,Entlebucher Mountain Dog
,Eskimo Dog
,Estrela Mountain Dog
,Eurasier
,Fell Terrier
,Field Spaniel
,Finnish Lapphund
,Finnish Spitz
,Flat Coated Retriever
,Formosan Mountain Dog
,Fox Hound
,Fox Terrier
,French Bulldog
,Frenchie Pug
,Galgo
,Gazelle Hound
,German Longhaired Pointer
,German Pointer
,German Shepherd
,German Shorthaired Pointer
,German Spitz
,German Wirehaired Pointer
,Giant Schnauzer
,Glen Of Imaal Terrier
,Golden Border Retriever
,Golden Labrador
,Golden Retriever
,Goldendoodle
,Gordon Setter
,Grand Bleu De Gascoigne
,Great Dane
,Greater Swiss Mountain Dog
,Greek Harehound
,Greek Sheepdog
,Greek Shepherd
,Greenland Dog
,Greyhound
,Griffon Brabancon
,Griffon Bruxellois
,Groenendael
,Hamiltonstovare
,Hanoverian Hound
,Harrier Hound
,Harrier Hound
,Havanese
,Hellenic Hound
,Hovawart
,Hungarian Kuvasz
,Hungarian Mudi
,Hungarian Puli
,Hungarian Pumi
,Hungarian Vizsla
,Hungarian Wirehaired Vizsla
,Husky
,Ibizan Hound
,Inuit
,Irish Red & White Setter
,Irish Red Setter
,Irish Setter
,Irish Terrier
,Irish Water Spaniel
,Irish Wolfhound
,Italian Greyhound
,Jack Chi
,Jack Russell Terrier
,Jack Tzu
,Jack-A-Bee
,Jack-A-Poo
,Jack-A-Ranian
,Jackadoodle
,Jackie-Bichon
,Jackshund
,Japanese Akita
,Japanese Chin
,Japanese Shiba Inu
,Japanese Spitz
,Jug
,Kangal dog
,Keeshond
,Kerry Blue Terrier
,King Charles Spaniel
,Komondor
,Kooikerhondje
,Korean Jindo
,Korthals Griffon
,Kromfohrlander
,Kyi-Leo
,Labpointer
,Labradoodle
,Labrador
,Labrador Retriever
,Labralas
,Labramarner
,Labrottie
,Lagotto Romagnolo
,Lakeland Terrier
,Lancashire Heeler
,Large Munsterlander
,Leonberger
,Lhajon
,Lhasa Apso
,Lhasapoo
,Libyan Desert Dog
,Llewellin Setter
,Lowchen (Little Lion Dog)
,Lucas Terrier
,Lurcher
,Majorca Shepherd Dog
,Mal-Shi
,Malinois
,Maltese
,Malti-Poo
,Maltichon
,Maltipom
,Maltipoo
,Manchester Terrier
,Maremma
,Mastiff
,Mexican Hairless
,Mi-Ki
,Minature Labradoodle
,Miniature Bull Terrier
,Miniature Dachshund
,Miniature Pinscher
,Miniature Poodle
,Miniature Schnauzer
,Miniature Shepherd Dog
,Morkie
,Neapolitan Mastiff
,New Zealand Huntaway
,New Zealand Red
,Newfoundland
,Newfypoo
,Norfolk Labrador
,Norfolk Terrier
,Norsk Lundehund
,Northern Inuit
,Norwegian Buhund
,Norwegian Elkhound
,Norwegian Lundehund
,Norwich Terrier
,Novascotia Duck Tolling Ret
,Old English Mastiff
,Old English Sheepdog
,Old English Bulldogge
,Otterhound
,Papastzu
,Papillon
,Parson Jack Russell Terrier
,Patterdale Terrier
,Patterjack
,Patterland
,Pekingese
,Pembroke Corgi
,Petit Basset Griffon Vendeen
,Phalene
,Pharaoh Hound
,Picardy Sheepdog
,Pinscher
,Plummer Terrier
,Pointer
,Polish Hunting Dog
,Polish Lowland Sheepdog
,Polish Tatra Sheepdog
,Pomapoo
,Pomeranian
,Pondenco Andaluz
,Poochon
,Poodle
,Porcelaine
,Portugese Water Dog
,Portuguese Podengo
,Portuguese Pointer
,Portuguese Sheepdog
,Prague Ratter
,Pug
,Pug-Zu
,Pugalier
,Puggle
,Pyrenean Mastiff
,Pyrenean Mountain Dog
,Pyrenean Sheepdog
,Rat Terrier
,Ratonero Bodeguero Andaluz
,Red Setter
,Redbone Coonhound
,Retriever
,Rhodesian Boxer
,Rhodesian Ridgeback
,Romanian Mioritic Shepherd Dog
,Rotterman
,Rottweiler
,Rough Collie
,Russian Black Terrier
,Russian Toy Terrier
,Saluki
,Samoyed
,Schipperke
,Schnauzer
,Schnoodle
,Scottish Terrier
,Sealyham Terrier
,Segugio Italiano
,Setter
,Sharpei
,Sheltie
,Shepadoodle
,Shetland Sheepdog
,Shiba Inu
,Shichi
,Shichon
,Shih Apso
,Shih Tzu
,Shih-Poo
,Shiloh Shepherd
,Shorkie Tzu
,Shug
,Siberian Husky
,Siberian Retriever
,Silken Windhound
,Skye Terrier
,Sloughis
,Slovakian Chuvac
,Slovakian Rough Haired Pointer
,Small Munsterlander
,Smooth Collie
,Soft Coated Wheaten Terrier
,Spangold Retriever
,Spaniel
,Spanish Mastiff
,Spanish Water Dog
,Spinone
,Spitz Mittel
,Springador
,Springer Spaniel
,Springerdoodle
,Springerpoo
,Sprocker Spaniel
,Sprollie
,St Bernard
,St Johns Waterdog
,Stabyhoun
,Staffordshire Bull Terrier
,Standard Poodle
,Sussex Spaniel
,Swedish Lapphund
,Swedish Vallhund
,Swiss Appenzelois
,Tamaskan
,Terrier
,Tervueren
,Thai Bangkaew Dog
,Thai Ridgeback
,Tiara Teddy Bear Dog
,Tibetan Mastiff
,Tibetan Spaniel
,Tibetan Terrier
,Toy Poodle
,Toy Rat Terrier
,Trailhound
,Treeing Walker Coonhound
,Utonagan
,Victorian Bulldog
,Volpino Italiano
,Weeranian
,Weimaraner
,Welsh Border Collie
,Welsh Corgi
,Welsh Sheepdog
,Welsh Springer Spaniel
,Welsh Terrier
,Weshi
,West Highland White Terrier
,Wheaten Terrier
,Whippet
,White Swiss Shepherd
,Wirehaired Dachshund
,Wirehaired Pointing Griffon
,Working Sheepdog
,Yellow Labrador
,Yoodle
,Yorkipoo
,Yorkshire Terrier
,Yorktese
,Yugoslavian Sarplaniniac".Split(',').ToList();
        }

        public static Pet CreateNewPet_01(IrishPetsDb _context, Member _member = null)
        {
            var __item = new Pet(_member ?? _context.Users.FirstOrDefault());

            var __breed = _context.PetBreeds.FirstOrDefault();
            if (null != __breed)
            {
                __item.BreedId = __breed.Id;
                __item.Breed = __breed;
            }

            __item.CoatColour = "Grey";
            __item.Name = "Dottie";
            __item.Note = "Hi, I'm Dottie, a pretty brown tabby who came here when my owner could no longer care for me.";
            __item.Weight = "2 kg";

            __item.DateOfBirth = DateTime.Now.AddDays(-30);
            __item.ExProperty = Pet_ExProperty.HealthTests;
            __item.Enabled = true;

            //_context.Pets.Add(__item);
            //_context.SaveChanges();

            return __item;
        }

        public static Pet CreateNewPet_02(IrishPetsDb _context, Member _member = null)
        {
            var __item = new Pet(_member ?? _context.Users.FirstOrDefault());

            var __breed = _context.PetBreeds.FirstOrDefault();
            if (null != __breed)
            {
                __item.BreedId = __breed.Id;
                __item.Breed = __breed;
            }

            __item.CoatColour = "Black";
            __item.Name = "Andy";
            __item.Note = "Hey! My name is Colt and I came here with my sister Fiona when our owner moved to a place that does not allow pets. ";
            __item.Weight = "2 kg";

            __item.DateOfBirth = DateTime.Now.AddDays(-30);
            __item.ExProperty = Pet_ExProperty.HealthTests;
            __item.Enabled = true;

            //_context.Pets.Add(__item);
            //_context.SaveChanges();

            return __item;
        }

        public static void CreateNewPetAdvert(IrishPetsDb _context, Member _member = null)
        {
            if (null != _context.PetAdverts.FirstOrDefault())
            {
                return;
            }

            var __item = new PetAdvert(CreateNewPet_01(_context, _member), AdvertType.Notification_Advert);
            __item.DateShowStart = DateTime.Now;
            __item.DateShowEnd = __item.DateShowStart.AddDays(30);
            __item.Name = "Smokey Silver Tabby kitten";
            __item.Note = "Available for collection. First vaccination provided. Litter trained.";
            __item.FirstPrice = 100;
            __item.TypeOfSale = TypeOfSale.Commercial;
            __item.IsShow = true;
            _context.PetAdverts.Add(__item);


            __item = new PetAdvert(CreateNewPet_02(_context, _member), AdvertType.Notification_LostAndFound);
            __item.DateShowStart = DateTime.Now;
            __item.DateShowEnd = __item.DateShowStart.AddDays(30);
            __item.Name = "Kitten lost";
            __item.Note = "Small, ~1 yr old female, brown/black/grey, swirling patterns on sides, green eyes, quiet. Has black spot just below rectum. Lost in Castlenock area on 30/09/2015. Information rewarded.";
            __item.FirstPrice = 100;
            __item.TypeOfSale = TypeOfSale.Commercial;
            __item.IsShow = true;
            _context.PetAdverts.Add(__item);

            _context.SaveChanges();
        }

        public static void CreateNew_PetService(IrishPetsDb _context)
        {
            if (null != _context.PetServices.FirstOrDefault())
            {
                return;
            }

            string __text = File.ReadAllText(@"IrishPets\bin\DataLoads\PetServices.json");

            var __jsonObject = JObject.Parse(__text);

            IList<JToken> __results = __jsonObject["PetServices"].Children().ToList();

            // serialize JSON results into .NET objects
            foreach (JToken _result in __results)
            {
                var __item = JsonConvert.DeserializeObject<PetService>(_result.ToString());

                _context.PetServices.Add(__item);
            }

            _context.SaveChanges();

            //var __item = new PetService();
            //__item.Name = "DSPCA Clinic";
            //__item.Note = "The Dublin Society for Prevention of Cruelty to Animals (DSPCA) is a registered charity, established in 1840 to prevent cruelty to animals and is now Ireland's largest animal welfare organisation.";
            //__item.Email = "vetclinic@dspca.ie";
            //__item.PhoneNumber = "+353 (1) 499 4780";
            //__item.Street = "Mount Venus Road";
            //__item.Town = "Rathfarnham";
            //__item.Postcode = "D16";
            //__item.CountyId = 9; // Dublin
            //__item.Enabled = true;
            //__item = _context.PetServices.Add(__item);

            //_context.SaveChanges();
        }
        
        public static void CreateNew_AdvAda(IrishPetsDb _context)
        {
            if (null != _context.AdvAdas.FirstOrDefault())
            {
                return;
            }

            string __text = File.ReadAllText(@"IrishPets\bin\DataLoads\AdvAdas.json");

            var __jsonObject = JObject.Parse(__text);

            IList<JToken> __results = __jsonObject["AdvAdas"].Children().ToList();

            // serialize JSON results into .NET objects
            foreach (JToken _result in __results)
            {
                var __item = JsonConvert.DeserializeObject<AdvAda>(_result.ToString());

                _context.AdvAdas.Add(__item);
            }

            _context.SaveChanges();
        }
    }
}