import { DataBufferReader, InsufficientCapacityException } from "../../src";

describe('DataBufferReaderTests', () => {

  describe('ReadBit', () => {
    it('should return the first bit from the first byte when ReadBit is called once', () => {
      const byteArray = new Uint8Array([0b10101100, 0b11010010]);
      const dataBuffer = new DataBufferReader(byteArray);

      const firstBit = dataBuffer.readBit();

      expect(firstBit).toBe(true);
    });

    it('should return the second bit from the first byte when ReadBit is called twice', () => {
      const byteArray = new Uint8Array([0b10101100, 0b11010010]);
      const dataBuffer = new DataBufferReader(byteArray);

      const firstBit = dataBuffer.readBit();
      const secondBit = dataBuffer.readBit();

      expect(firstBit).toBe(true);
      expect(secondBit).toBe(false);
    });

    it('should throw error when reading bits exceeds array length', () => {
      const byteArray = new Uint8Array([0b10101100]);
      const dataBuffer = new DataBufferReader(byteArray);

      for (let i = 0; i < 8; i++) {
        dataBuffer.readBit();
      }

      expect(() => dataBuffer.readBit()).toThrow(InsufficientCapacityException);
    });
  });

  describe('ReadByte', () => {
    it('should return the first byte when ReadByte is called once', () => {
      const byteArray = new Uint8Array([250]);
      const dataBuffer = new DataBufferReader(byteArray);

      const readFirst = dataBuffer.readByte();

      expect(readFirst).toBe(250);
    });

    it('should return the second byte when ReadByte is called twice', () => {
      const byteArray = new Uint8Array([250, 175]);
      const dataBuffer = new DataBufferReader(byteArray);

      const readFirst = dataBuffer.readByte();
      const readSecond = dataBuffer.readByte();

      expect(readFirst).toBe(250);
      expect(readSecond).toBe(175);
    });

    it('should throw error when reading bytes exceeds array length', () => {
      const byteArray = new Uint8Array([250, 75]);
      const dataBuffer = new DataBufferReader(byteArray);

      dataBuffer.readByte();
      dataBuffer.readByte();

      expect(() => dataBuffer.readByte()).toThrow(InsufficientCapacityException);
    });
  });

  describe('ReadBytes', () => {
    it('should return the complete array when ReadBytes is called with the same length as the array', () => {
      const byteArray = new Uint8Array([250, 175, 100]);
      const dataBuffer = new DataBufferReader(byteArray);

      const readBytes = dataBuffer.readBytes(3);

      expect(readBytes).toEqual(new Uint8Array([250, 175, 100]));
    });

    it('should return a partial array when ReadBytes is called with less than the array length', () => {
      const byteArray = new Uint8Array([250, 175, 100]);
      const dataBuffer = new DataBufferReader(byteArray);

      const readBytes = dataBuffer.readBytes(2);

      expect(readBytes).toEqual(new Uint8Array([250, 175]));
    });

    it('should throw error when reading bytes exceeds array length', () => {
      const byteArray = new Uint8Array([250, 175]);
      const dataBuffer = new DataBufferReader(byteArray);

      expect(() => dataBuffer.readBytes(3)).toThrow(InsufficientCapacityException);
    });
  });

  describe('Multiple - ReadBit + ReadByte', () => {
    it('should return correct data when reading one bit followed by one byte', () => {
      const byteArray = new Uint8Array([0b01111111, 0b10000000]);
      const dataBuffer = new DataBufferReader(byteArray);

      const readBit = dataBuffer.readBit();
      const readByte = dataBuffer.readByte();

      expect(readBit).toBe(false);
      expect(readByte).toBe(0b11111111);
    });

    it('should return correct data when reading two bits followed by one byte', () => {
      const byteArray = new Uint8Array([0b00111111, 0b11000000]);
      const dataBuffer = new DataBufferReader(byteArray);

      const readBitOne = dataBuffer.readBit();
      const readBitTwo = dataBuffer.readBit();
      const readByte = dataBuffer.readByte();

      expect(readBitOne).toBe(false);
      expect(readBitTwo).toBe(false);
      expect(readByte).toBe(0b11111111);
    });

    it('should return correct data when reading one bit followed by two bytes', () => {
      const byteArray = new Uint8Array([0b01111111, 0b11111111, 0b10000000]);
      const dataBuffer = new DataBufferReader(byteArray);

      const readBit = dataBuffer.readBit();
      const readByteOne = dataBuffer.readByte();
      const readByteTwo = dataBuffer.readByte();

      expect(readBit).toBe(false);
      expect(readByteOne).toBe(0b11111111);
      expect(readByteTwo).toBe(0b11111111);
    });

    it('should return correct data when reading one bit, one byte, one bit, and one byte', () => {
      const byteArray = new Uint8Array([0b01111111, 0b10111111, 0b11000000]);
      const dataBuffer = new DataBufferReader(byteArray);

      const readBitOne = dataBuffer.readBit();
      const readByteOne = dataBuffer.readByte();
      const readBitTwo = dataBuffer.readBit();
      const readByteTwo = dataBuffer.readByte();

      expect(readBitOne).toBe(false);
      expect(readByteOne).toBe(0b11111111);
      expect(readBitTwo).toBe(false);
      expect(readByteTwo).toBe(0b11111111);
    });

    it('should return correct data when reading one byte, one bit, and one byte', () => {
      const byteArray = new Uint8Array([0b11111111, 0b01111111, 0b10000000]);
      const dataBuffer = new DataBufferReader(byteArray);

      const readByteOne = dataBuffer.readByte();
      const readBit = dataBuffer.readBit();
      const readByteTwo = dataBuffer.readByte();

      expect(readByteOne).toBe(0b11111111);
      expect(readBit).toBe(false);
      expect(readByteTwo).toBe(0b11111111);
    });
  });

  describe('Multiple - ReadBit + ReadBytes', () => {
    it('should return correct data when reading one bit followed by one byte array', () => {
      const byteArray = new Uint8Array([0b10000001, 0b10000000]);
      const dataBuffer = new DataBufferReader(byteArray);

      const readBit = dataBuffer.readBit();
      const readBytes = dataBuffer.readBytes(1);

      expect(readBit).toBe(true);
      expect(readBytes).toEqual(new Uint8Array([0b00000011]));
    });

    it('should return correct data when reading one bit followed by two byte array', () => {
      const byteArray = new Uint8Array([0b10000001, 0b10000011, 0b11111111]);
      const dataBuffer = new DataBufferReader(byteArray);

      const readBit = dataBuffer.readBit();
      const readBytes = dataBuffer.readBytes(2);

      expect(readBit).toBe(true);
      expect(readBytes).toEqual(new Uint8Array([0b00000011, 0b00000111]));
    });

    it('should return correct data when reading one bit followed by three byte array', () => {
      const byteArray = new Uint8Array([0b10000001, 0b10000011, 0b11111111, 0b00000000]);
      const dataBuffer = new DataBufferReader(byteArray);

      const readBit = dataBuffer.readBit();
      const readBytes = dataBuffer.readBytes(3);

      expect(readBit).toBe(true);
      expect(readBytes).toEqual(new Uint8Array([0b00000011, 0b00000111, 0b11111110]));
    });

    it('should return correct data when reading one byte array followed by one bit', () => {
      const byteArray = new Uint8Array([0b10000001, 0b10000011, 0b11111111]);
      const dataBuffer = new DataBufferReader(byteArray);

      const readBytes = dataBuffer.readBytes(2);
      const readBit = dataBuffer.readBit();

      expect(readBytes).toEqual(new Uint8Array([0b10000001, 0b10000011]));
      expect(readBit).toBe(true);
    });

    it('should return correct data when reading two byte array followed by one bit', () => {
      const byteArray = new Uint8Array([0b10000001, 0b10000011, 0b11111111, 0b00000000]);
      const dataBuffer = new DataBufferReader(byteArray);

      const readBytes = dataBuffer.readBytes(3);
      const readBit = dataBuffer.readBit();

      expect(readBytes).toEqual(new Uint8Array([0b10000001, 0b10000011, 0b11111111]));
      expect(readBit).toBe(false);
    });
  });

  describe('Multiple - ReadByte + ReadBytes', () => {
    it('should return correct data when reading one byte followed by two byte array', () => {
      const byteArray = new Uint8Array([0b10000001, 0b10000011, 0b11111111]);
      const dataBuffer = new DataBufferReader(byteArray);

      const readByte = dataBuffer.readByte();
      const readBytes = dataBuffer.readBytes(2);

      expect(readByte).toBe(0b10000001);
      expect(readBytes).toEqual(new Uint8Array([0b10000011, 0b11111111]));
    });

    it('should return correct data when reading two byte array followed by one byte', () => {
      const byteArray = new Uint8Array([0b10111101, 0b11001110, 0b10101100, 0b00000000]);
      const dataBuffer = new DataBufferReader(byteArray);

      const readBytes = dataBuffer.readBytes(2);
      const readByte = dataBuffer.readByte();

      expect(readBytes).toEqual(new Uint8Array([0b10111101, 0b11001110]));
      expect(readByte).toBe(0xAC);
    });

    it('should return correct data when reading two bytes followed by one byte array', () => {
      const byteArray = new Uint8Array([0b10000001, 0b10000011, 0b11111111, 0b00000000]);
      const dataBuffer = new DataBufferReader(byteArray);

      const readByteOne = dataBuffer.readByte();
      const readByteTwo = dataBuffer.readByte();
      const readBytes = dataBuffer.readBytes(2);

      expect(readByteOne).toBe(0b10000001);
      expect(readByteTwo).toBe(0b10000011);
      expect(readBytes).toEqual(new Uint8Array([0b11111111, 0b00000000]));
    });
  });

  describe('Multiple - ReadBit + ReadByte + ReadBytes', () => {
    it('should return correct data when reading one bit, one byte, two byte array, and one bit', () => {
      const byteArray = new Uint8Array([0b11111111, 0b11010110, 0b01011110, 0b10000000]);
      const dataBuffer = new DataBufferReader(byteArray);

      const readBit1 = dataBuffer.readBit();
      const readByte = dataBuffer.readByte();
      const readBytes = dataBuffer.readBytes(2);
      const readBit2 = dataBuffer.readBit();

      expect(readBit1).toBe(true);
      expect(readByte).toBe(0b11111111);
      expect(readBytes).toEqual(new Uint8Array([0b10101100, 0b10111101]));
      expect(readBit2).toBe(false);
    });
  });

  describe('Bookmark', () => {
    it('should return the same byte after bookmark disposal', () => {
      const byteArray = new Uint8Array([250, 175, 100]);
      const dataBuffer = new DataBufferReader(byteArray);

      expect(dataBuffer.readByte()).toBe(250);

      const bookmark = dataBuffer.bookmark();
      expect(dataBuffer.readByte()).toBe(175);
      bookmark.dispose();

      expect(dataBuffer.readByte()).toBe(175);
      expect(dataBuffer.readByte()).toBe(100);
    });
  });
});