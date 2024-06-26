import xlrd
import xlwt
import secrets
import os
from Crypto.Cipher import AES
from Crypto.Random import get_random_bytes
from Crypto.Util.Padding import pad, unpad

def encrypt_content(content, cipher):               #create AES cipher object with random key
    
    padded_content = pad(content.encode('utf-8'), AES.block_size)
    encrypted_content = cipher.encrypt(padded_content)
    
    return encrypted_content

def decrypt_content(content, cipher):

    decrypted_content = unpad(cipher.decrypt(content), AES.block_size)
    
    return decrypted_content.decode()

def create_key(key_file_path):
    encryption_key = secrets.token_bytes(16)            #16 random bytes
    cipher = AES.new(encryption_key, AES.MODE_CBC)
    with open(key_file_path, 'wb')as c_file:
        c_file.write(encryption_key)

    return

def encrypt_excel_file(input_file_path):                #enc key created before encryption

    encryption_key = secrets.token_bytes(16)            #16 random bytes
    cipher = AES.new(encryption_key, AES.MODE_CBC)
    with open('cipher_file', 'wb')as c_file:
        c_file.write(encryption_key)
    
    workbook = xlrd.open_workbook(input_file_path)

    new_workbook = xlwt.Workbook()
    
    for sheet_index in range(workbook.nsheets):         #each sheet
        sheet = workbook.sheet_by_index(sheet_index)
        new_sheet = new_workbook.add_sheet(sheet.name)
        for row_index in range(sheet.nrows):            #each row
            for col_index in range(sheet.ncols):        #each cell
                
                cell_content = sheet.cell_value(row_index, col_index)
                encrypted_content = encrypt_content(str(cell_content), cipher)
                
                new_sheet.write(row_index, col_index, encrypted_content.hex())      #write encrypted values

    new_sheet = new_workbook.add_sheet(" ")     #append iv to hidden sheet
    new_sheet.write(0,0,cipher.iv.hex())
    new_sheet.visibility = 1
    
    new_workbook.save(input_file_path)
    return

def encrypt_excel_file_with_key(input_file_path, key_file_path):      #enc key supplied by user

    with open(key_file_path, 'rb')as c_file:
        encryption_key = c_file.read()
    cipher = AES.new(encryption_key, AES.MODE_CBC)
    
    workbook = xlrd.open_workbook(input_file_path)

    new_workbook = xlwt.Workbook()
    
    for sheet_index in range(workbook.nsheets):         #each sheet
        sheet = workbook.sheet_by_index(sheet_index)
        new_sheet = new_workbook.add_sheet(sheet.name)
        for row_index in range(sheet.nrows):            #each row
            for col_index in range(sheet.ncols):        #each cell
                
                cell_content = sheet.cell_value(row_index, col_index)
                encrypted_content = encrypt_content(str(cell_content), cipher)
                
                new_sheet.write(row_index, col_index, encrypted_content.hex())      #write encrypted values

    new_sheet = new_workbook.add_sheet(" ")     #append iv to hidden sheet
    new_sheet.write(0,0,cipher.iv.hex())
    new_sheet.visibility = 1
    
    new_workbook.save(input_file_path)
    return

def decrypt_excel_file_with_key(input_file_path, key_file_path):

    workbook = xlrd.open_workbook(input_file_path)

    last_sheet_index = workbook.nsheets - 1
    last_sheet = workbook.sheet_by_index(last_sheet_index)
    iv_hex = last_sheet.cell_value(0, 0)
    iv = bytes.fromhex(iv_hex)

    with open(key_file_path, 'rb')as c_file:
        encryption_key = c_file.read()
    cipher = AES.new(encryption_key, AES.MODE_CBC, iv)
  
    new_workbook = xlwt.Workbook()
    
    for sheet_index in range(workbook.nsheets - 1):         #each sheet
        sheet = workbook.sheet_by_index(sheet_index)
        new_sheet = new_workbook.add_sheet(sheet.name)
        for row_index in range(sheet.nrows):            #each row
            for col_index in range(sheet.ncols):        #each cell

                cell_content = sheet.cell_value(row_index, col_index)
                decrypted_content = decrypt_content(bytes.fromhex(cell_content), cipher)
                
                new_sheet.write(row_index, col_index, decrypted_content)
    
    new_workbook.save(input_file_path)
    return