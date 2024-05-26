import json
import random
from faker import Faker

fake = Faker('uk_UA')

students = []

for _ in range(200):
    name = fake.first_name()
    surname = fake.last_name()
    patronimic = fake.middle_name()
    email = fake.email()
    phone = fake.phone_number()
    birth_date = fake.date_of_birth(minimum_age=17, maximum_age=21).strftime('%d.%m.%Y')
    
    semesters = []
    for semester_number in range(5, 9):
        subject_ids = random.sample(range(1, 201), 6)  # Generate 6 unique subject IDs
        subjects = [{"id": subject_id, "grade": random.randint(60, 100)} for subject_id in subject_ids]
        semesters.append({"semesterNumber": semester_number, "subjects": subjects})
    
    student = {
        "name": name,
        "surname": surname,
        "patronimic": patronimic,
        "email": email,
        "phone": phone,
        "birthDate": birth_date,
        "semesters": random.sample(semesters, random.randint(1, 4))  # Random number of semesters from 1 to 4
    }
    
    students.append(student)

with open('studentsData.json', 'w', encoding='utf-8') as f:
    json.dump(students, f, ensure_ascii=False, indent=4)
