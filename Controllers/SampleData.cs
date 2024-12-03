using Storefront.DbContext;

namespace Storefront.Controllers;

public static class SampleData
{
    public static List<Contact> Contacts()
    {
        return
        [
            new()
            {
                Avatar =
                    "https://sessionize.com/image/124e-400o400o2-wHVdAuNaxi8KJrgtN3ZKci.jpg",
                FirstName = "Shruti",
                LastName = "Kapoor",
                Twitter = "@shrutikapoor08",
            },
            new()
            {
                Avatar =
                    "https://sessionize.com/image/1940-400o400o2-Enh9dnYmrLYhJSTTPSw3MH.jpg",
                FirstName = "Glenn",
                LastName = "Reyes",
                Twitter = "@glnnrys",
            },
            new()
            {
                Avatar =
                    "https://sessionize.com/image/9273-400o400o2-3tyrUE3HjsCHJLU5aUJCja.jpg",
                FirstName = "Ryan",
                LastName = "Florence",
            },
            new()
            {
                Avatar =
                    "https://sessionize.com/image/d14d-400o400o2-pyB229HyFPCnUcZhHf3kWS.png",
                FirstName = "Oscar",
                LastName = "Newman",
                Twitter = "@__oscarnewman",
            },
            new()
            {
                Avatar =
                    "https://sessionize.com/image/fd45-400o400o2-fw91uCdGU9hFP334dnyVCr.jpg",
                FirstName = "Michael",
                LastName = "Jackson",
            },
            new()
            {
                Avatar =
                    "https://sessionize.com/image/b07e-400o400o2-KgNRF3S9sD5ZR4UsG7hG4g.jpg",
                FirstName = "Christopher",
                LastName = "Chedeau",
                Twitter = "@Vjeux",
            },
            new()
            {
                Avatar =
                    "https://sessionize.com/image/262f-400o400o2-UBPQueK3fayaCmsyUc1Ljf.jpg",
                FirstName = "Cameron",
                LastName = "Matheson",
                Twitter = "@cmatheson",
            },
            new()
            {
                Avatar =
                    "https://sessionize.com/image/820b-400o400o2-Ja1KDrBAu5NzYTPLSC3GW8.jpg",
                FirstName = "Brooks",
                LastName = "Lybrand",
                Twitter = "@BrooksLybrand",
            },
            new()
            {
                Avatar =
                    "https://sessionize.com/image/df38-400o400o2-JwbChVUj6V7DwZMc9vJEHc.jpg",
                FirstName = "Alex",
                LastName = "Anderson",
                Twitter = "@ralex1993",
            },
            new()
            {
                Avatar =
                    "https://sessionize.com/image/5578-400o400o2-BMT43t5kd2U1XstaNnM6Ax.jpg",
                FirstName = "Kent C.",
                LastName = "Dodds",
                Twitter = "@kentcdodds",
            },
            new()
            {
                Avatar =
                    "https://sessionize.com/image/c9d5-400o400o2-Sri5qnQmscaJXVB8m3VBgf.jpg",
                FirstName = "Nevi",
                LastName = "Shah",
                Twitter = "@nevikashah",
            },
            new()
            {
                Avatar =
                    "https://sessionize.com/image/2694-400o400o2-MYYTsnszbLKTzyqJV17w2q.png",
                FirstName = "Andrew",
                LastName = "Petersen",
            },
            new()
            {
                Avatar =
                    "https://sessionize.com/image/907a-400o400o2-9TM2CCmvrw6ttmJiTw4Lz8.jpg",
                FirstName = "Scott",
                LastName = "Smerchek",
                Twitter = "@smerchek",
            },
            new()
            {
                Avatar =
                    "https://sessionize.com/image/08be-400o400o2-WtYGFFR1ZUJHL9tKyVBNPV.jpg",
                FirstName = "Giovanni",
                LastName = "Benussi",
                Twitter = "@giovannibenussi",
            },
            new()
            {
                Avatar =
                    "https://sessionize.com/image/f814-400o400o2-n2ua5nM9qwZA2hiGdr1T7N.jpg",
                FirstName = "Igor",
                LastName = "Minar",
                Twitter = "@IgorMinar",
            },
            new()
            {
                Avatar =
                    "https://sessionize.com/image/fb82-400o400o2-LbvwhTVMrYLDdN3z4iEFMp.jpeg",
                FirstName = "Brandon",
                LastName = "Kish",
            },
            new()
            {
                Avatar =
                    "https://sessionize.com/image/fcda-400o400o2-XiYRtKK5Dvng5AeyC8PiUA.png",
                FirstName = "Arisa",
                LastName = "Fukuzaki",
                Twitter = "@arisa_dev",
            },
            new()
            {
                Avatar =
                    "https://sessionize.com/image/c8c3-400o400o2-PR5UsgApAVEADZRixV4H8e.jpeg",
                FirstName = "Alexandra",
                LastName = "Spalato",
                Twitter = "@alexadark",
            },
            new()
            {
                Avatar =
                    "https://sessionize.com/image/7594-400o400o2-hWtdCjbdFdLgE2vEXBJtyo.jpg",
                FirstName = "Cat",
                LastName = "Johnson",
            },
            new()
            {
                Avatar =
                    "https://sessionize.com/image/5636-400o400o2-TWgi8vELMFoB3hB9uPw62d.jpg",
                FirstName = "Ashley",
                LastName = "Narcisse",
                Twitter = "@_darkfadr",
            },
            new()
            {
                Avatar =
                    "https://sessionize.com/image/6aeb-400o400o2-Q5tAiuzKGgzSje9ZsK3Yu5.JPG",
                FirstName = "Edmund",
                LastName = "Hung",
                Twitter = "@_edmundhung",
            },
            new()
            {
                Avatar =
                    "https://sessionize.com/image/30f1-400o400o2-wJBdJ6sFayjKmJycYKoHSe.jpg",
                FirstName = "Clifford",
                LastName = "Fajardo",
                Twitter = "@cliffordfajard0",
            },
            new()
            {
                Avatar =
                    "https://sessionize.com/image/6faa-400o400o2-amseBRDkdg7wSK5tjsFDiG.jpg",
                FirstName = "Erick",
                LastName = "Tamayo",
                Twitter = "@ericktamayo",
            },
            new()
            {
                Avatar =
                    "https://sessionize.com/image/feba-400o400o2-R4GE7eqegJNFf3cQ567obs.jpg",
                FirstName = "Paul",
                LastName = "Bratslavsky",
                Twitter = "@codingthirty",
            },
            new()
            {
                Avatar =
                    "https://sessionize.com/image/c315-400o400o2-spjM5A6VVfVNnQsuwvX3DY.jpg",
                FirstName = "Pedro",
                LastName = "Cattori",
                Twitter = "@pcattori",
            },
            new()
            {
                Avatar =
                    "https://sessionize.com/image/eec1-400o400o2-HkvWKLFqecmFxLwqR9KMRw.jpg",
                FirstName = "Andre",
                LastName = "Landgraf",
                Twitter = "@AndreLandgraf94",
            },
            new()
            {
                Avatar =
                    "https://sessionize.com/image/c73a-400o400o2-4MTaTq6ftC15hqwtqUJmTC.jpg",
                FirstName = "Monica",
                LastName = "Powell",
                Twitter = "@indigitalcolor",
            },
            new()
            {
                Avatar =
                    "https://sessionize.com/image/cef7-400o400o2-KBZUydbjfkfGACQmjbHEvX.jpeg",
                FirstName = "Brian",
                LastName = "Lee",
                Twitter = "@brian_dlee",
            },
            new()
            {
                Avatar =
                    "https://sessionize.com/image/f83b-400o400o2-Pyw3chmeHMxGsNoj3nQmWU.jpg",
                FirstName = "Sean",
                LastName = "McQuaid",
                Twitter = "@SeanMcQuaidCode",
            },
            new()
            {
                Avatar =
                    "https://sessionize.com/image/a9fc-400o400o2-JHBnWZRoxp7QX74Hdac7AZ.jpg",
                FirstName = "Shane",
                LastName = "Walker",
                Twitter = "@swalker326",
            },
            new()
            {
                Avatar =
                    "https://sessionize.com/image/6644-400o400o2-aHnGHb5Pdu3D32MbfrnQbj.jpg",
                FirstName = "Jon",
                LastName = "Jensen",
                Twitter = "@jenseng",
            }
        ];
    }
}