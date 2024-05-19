import json
import random
from faker import Faker

fake = Faker('uk_UA')

students = [
    {
        "name": "Антон",
        "surname": "Антоненко",
        "patronimic": "Антонович",
        "email": "ands2131@hello.com",
        "phone": "+888 9012 9 81928",
        "birthDate": "23.01.2003",
        "semesters": [
            {
                "semesterNumber": 5,
                "subjects": [
                    {"id": 1, "grade": 75},
                    {"id": 2, "grade": 82},
                    {"id": 4, "grade": 90}
                ]
            }
        ]
    },
    {
        "name": "Олексій",
        "surname": "Коваленко",
        "patronimic": "Васильович",
        "email": "oleksiy.kovalenko@example.com",
        "phone": "+380 98765 4321",
        "birthDate": "15.06.2003",
        "semesters": [
            {
                "semesterNumber": 5,
                "subjects": [
                    {"id": 1, "grade": 82},
                    {"id": 2, "grade": 75},
                    {"id": 3, "grade": 60}
                ]
            }          
        ]
    }
]

for _ in range(198):
    name = fake.first_name()
    surname = fake.last_name()
    patronimic = fake.middle_name()
    email = fake.email()
    phone = fake.phone_number()
    birth_date = fake.date_of_birth(minimum_age=17, maximum_age=21).strftime('%d.%m.%Y')
    
    semesters = []
    for semester_number in range(5, 9):
        subjects = [{"id": random.randint(1, 200), "grade": random.randint(60, 100)} for _ in range(6)]
        semesters.append({"semesterNumber": semester_number, "subjects": subjects})
    
    student = {
        "name": name,
        "surname": surname,
        "patronimic": patronimic,
        "email": email,
        "phone": phone,
        "birthDate": birth_date,
        "semesters": random.sample(semesters, random.randint(1, 4)) # Random number of semesters from 1 to 4
    }
    
    students.append(student)

with open('studentsData.json', 'w', encoding='utf-8') as f:
    json.dump(students, f, ensure_ascii=False, indent=4)
