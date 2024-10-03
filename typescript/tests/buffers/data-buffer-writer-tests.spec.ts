import { DataBufferWriter, InsufficientCapacityException } from "../../src";

describe('DataBufferWriterTests', () => {

  function getBit(byte: number, bitPosition: number): boolean {
    return (byte & (1 << (7 - bitPosition))) !== 0;
  }

  function printBits(byte: number): string {
    return byte.toString(2).padStart(8, '0');
  }

  const bitCases: boolean[][] = [
    [true, true],
    [false, false],
    [true, true, false, true],
    [false, false, true, false],
    [true, false, true, false, true, false, true, false],
    [false, true, false, true, false, true, false, true]
  ];

  describe('WriteBit', () => {

    it('should modify buffer when writing true', () => {
      const byteArray = new Uint8Array(1);
      const dataBuffer = new DataBufferWriter(byteArray);

      dataBuffer.writeBit(true);

      expect(getBit(byteArray[0], 0)).toBe(true);
    });

    it('should modify buffer when writing false', () => {
      const byteArray = new Uint8Array(1);
      const dataBuffer = new DataBufferWriter(byteArray);

      dataBuffer.writeBit(false);

      expect(getBit(byteArray[0], 0)).toBe(false);
    });

    it.each(bitCases)('should modify buffer when writing multiple bits', (...bits) => {
      const byteArray = new Uint8Array(1);
      const dataBuffer = new DataBufferWriter(byteArray);

      bits.forEach(bit => dataBuffer.writeBit(bit));

      bits.forEach((bit, i) => {
        expect(getBit(byteArray[0], i)).toBe(bit);
      });
    });

    it('should throw error when writing bit exceeds array length', () => {
      const byteArray = new Uint8Array(1);
      const dataBuffer = new DataBufferWriter(byteArray);

      for (let i = 0; i < 8; i++) {
        dataBuffer.writeBit(true);
      }

      expect(() => dataBuffer.writeBit(false)).toThrow(InsufficientCapacityException);
    });
  });

  describe('WriteByte', () => {

    it('should modify buffer when writing one byte', () => {
      const byteArray = new Uint8Array(1);
      const dataBuffer = new DataBufferWriter(byteArray);
      const valueToWrite = 0xAC;

      dataBuffer.writeByte(valueToWrite);

      expect(byteArray[0]).toBe(valueToWrite);
    });

    it('should throw error when writing byte exceeds array length', () => {
      const byteArray = new Uint8Array(1);
      const dataBuffer = new DataBufferWriter(byteArray);

      dataBuffer.writeByte(0xFF);

      expect(() => dataBuffer.writeByte(0xFF)).toThrow(InsufficientCapacityException);
    });
  });

  describe('WriteBytes', () => {

    it('should modify buffer when writing three bytes', () => {
      const byteArray = new Uint8Array(3);
      const dataBuffer = new DataBufferWriter(byteArray);
      const valuesToWrite = new Uint8Array([0xAC, 0xBD, 0xCE]);

      dataBuffer.writeBytes(valuesToWrite);

      expect(byteArray).toEqual(valuesToWrite);
    });

    it('should throw error when writing bytes exceeds array length', () => {
      const byteArray = new Uint8Array(2);
      const dataBuffer = new DataBufferWriter(byteArray);
      const valuesToWrite = new Uint8Array([0xAC, 0xBD, 0xCE]);

      expect(() => dataBuffer.writeBytes(valuesToWrite)).toThrow(InsufficientCapacityException);
    });
  });

  describe('WriteBit + WriteByte', () => {

    it('should modify buffer when writing one bit followed by one byte', () => {
      const byteArray = new Uint8Array(2);
      const dataBuffer = new DataBufferWriter(byteArray);

      dataBuffer.writeBit(false);
      dataBuffer.writeByte(0b11111111);

      expect(byteArray[0]).toBe(0b01111111);
      expect(byteArray[1]).toBe(0b10000000);
    });

    it('should modify buffer when writing two bits followed by one byte', () => {
      const byteArray = new Uint8Array(2);
      const dataBuffer = new DataBufferWriter(byteArray);

      dataBuffer.writeBit(false);
      dataBuffer.writeBit(false);
      dataBuffer.writeByte(0b11111111);

      expect(byteArray[0]).toBe(0b00111111);
      expect(byteArray[1]).toBe(0b11000000);
    });

    it('should modify buffer when writing one bit followed by two bytes', () => {
      const byteArray = new Uint8Array(3);
      const dataBuffer = new DataBufferWriter(byteArray);

      dataBuffer.writeBit(false);
      dataBuffer.writeByte(0b11111111);
      dataBuffer.writeByte(0b11111111);

      expect(byteArray[0]).toBe(0b01111111);
      expect(byteArray[1]).toBe(0b11111111);
      expect(byteArray[2]).toBe(0b10000000);
    });

    it('should modify buffer when writing one bit followed by one byte followed by one bit followed by one byte', () => {
      const byteArray = new Uint8Array(3);
      const dataBuffer = new DataBufferWriter(byteArray);

      dataBuffer.writeBit(false);
      dataBuffer.writeByte(0b11111111);
      dataBuffer.writeBit(false);
      dataBuffer.writeByte(0b11111111);

      expect(byteArray[0]).toBe(0b01111111);
      expect(byteArray[1]).toBe(0b10111111);
      expect(byteArray[2]).toBe(0b11000000);
    });

    it('should modify buffer when writing one byte followed by one bit followed by one byte', () => {
      const byteArray = new Uint8Array(3);
      const dataBuffer = new DataBufferWriter(byteArray);

      dataBuffer.writeByte(0b11111111);
      dataBuffer.writeBit(false);
      dataBuffer.writeByte(0b11111111);

      expect(byteArray[0]).toBe(0b11111111);
      expect(byteArray[1]).toBe(0b01111111);
      expect(byteArray[2]).toBe(0b10000000);
    });
  });

  describe('WriteBit + WriteBytes', () => {

    it('should modify buffer when writing one bit followed by one byte array', () => {
      const byteArray = new Uint8Array(2);
      const dataBuffer = new DataBufferWriter(byteArray);

      dataBuffer.writeBit(true);
      dataBuffer.writeBytes(new Uint8Array([0b00000011]));

      expect(byteArray[0]).toBe(0b10000001);
      expect(byteArray[1]).toBe(0b10000000);
    });

    it('should modify buffer when writing one bit followed by two byte array', () => {
      const byteArray = new Uint8Array(3);
      const dataBuffer = new DataBufferWriter(byteArray);

      dataBuffer.writeBit(true);
      dataBuffer.writeBytes(new Uint8Array([0b00000011, 0b00000111]));

      expect(byteArray[0]).toBe(0b10000001);
      expect(byteArray[1]).toBe(0b10000011);
      expect(byteArray[2]).toBe(0b10000000);
    });

    it('should modify buffer when writing one bit followed by three byte array', () => {
      const byteArray = new Uint8Array(4);
      const dataBuffer = new DataBufferWriter(byteArray);

      dataBuffer.writeBit(true);
      dataBuffer.writeBytes(new Uint8Array([0b00000011, 0b00000111, 0b11111110]));

      expect(byteArray[0]).toBe(0b10000001);
      expect(byteArray[1]).toBe(0b10000011);
      expect(byteArray[2]).toBe(0b11111111);
      expect(byteArray[3]).toBe(0b00000000);
    });

    it('should modify buffer when writing one byte array followed by one bit', () => {
      const byteArray = new Uint8Array(3);
      const dataBuffer = new DataBufferWriter(byteArray);

      dataBuffer.writeBytes(new Uint8Array([0b10000001, 0b10000011]));
      dataBuffer.writeBit(true);

      expect(byteArray[0]).toBe(0b10000001);
      expect(byteArray[1]).toBe(0b10000011);
      expect(byteArray[2]).toBe(0b10000000);
    });

    it('should modify buffer when writing two byte array followed by one bit', () => {
      const byteArray = new Uint8Array(4);
      const dataBuffer = new DataBufferWriter(byteArray);

      dataBuffer.writeBytes(new Uint8Array([0b10000001, 0b10000011, 0b11111111]));
      dataBuffer.writeBit(false);

      expect(byteArray[0]).toBe(0b10000001);
      expect(byteArray[1]).toBe(0b10000011);
      expect(byteArray[2]).toBe(0b11111111);
      expect(byteArray[3]).toBe(0b00000000);
    });

    it('should throw error when writing one bit followed by two byte array', () => {
      const byteArray = new Uint8Array(2);
      const dataBuffer = new DataBufferWriter(byteArray);

      dataBuffer.writeBit(true);

      expect(() => dataBuffer.writeBytes(new Uint8Array([0b00000011, 0b00000111]))).toThrow(InsufficientCapacityException);
    });

    it('should throw error when writing one bit followed by three byte array', () => {
      const byteArray = new Uint8Array(3);
      const dataBuffer = new DataBufferWriter(byteArray);

      dataBuffer.writeBit(true);

      expect(() => dataBuffer.writeBytes(new Uint8Array([0b00000011, 0b00000111, 0b11111110]))).toThrow(InsufficientCapacityException);
    });
  });

  describe('WriteByte + WriteBytes', () => {

    it('should modify buffer when writing one byte followed by two byte array', () => {
      const byteArray = new Uint8Array(4);
      const dataBuffer = new DataBufferWriter(byteArray);

      dataBuffer.writeByte(0b10000001);
      dataBuffer.writeBytes(new Uint8Array([0b10000011, 0b11111111]));

      expect(byteArray[0]).toBe(0b10000001);
      expect(byteArray[1]).toBe(0b10000011);
      expect(byteArray[2]).toBe(0b11111111);
      expect(byteArray[3]).toBe(0b00000000);
    });

    it('should modify buffer when writing two byte array followed by one byte', () => {
      const byteArray = new Uint8Array(4);
      const dataBuffer = new DataBufferWriter(byteArray);

      dataBuffer.writeBytes(new Uint8Array([0xBD, 0xCE]));
      dataBuffer.writeByte(0xAC);

      expect(byteArray[0]).toBe(0xBD);
      expect(byteArray[1]).toBe(0xCE);
      expect(byteArray[2]).toBe(0xAC);
      expect(byteArray[3]).toBe(0x00);
    });

    it('should modify buffer when writing two bytes followed by one byte array', () => {
      const byteArray = new Uint8Array(4);
      const dataBuffer = new DataBufferWriter(byteArray);

      dataBuffer.writeByte(0b10000001);
      dataBuffer.writeByte(0b10000011);
      dataBuffer.writeBytes(new Uint8Array([0b11111111, 0b00000000]));

      expect(byteArray[0]).toBe(0b10000001);
      expect(byteArray[1]).toBe(0b10000011);
      expect(byteArray[2]).toBe(0b11111111);
      expect(byteArray[3]).toBe(0b00000000);
    });
  });

  describe('WriteBit + WriteByte + WriteBytes', () => {

    it('should modify buffer when writing one bit followed by one byte followed by two byte array followed by one bit', () => {
      const byteArray = new Uint8Array(4);
      const dataBuffer = new DataBufferWriter(byteArray);

      dataBuffer.writeBit(true);
      dataBuffer.writeByte(0b11111111);
      dataBuffer.writeBytes(new Uint8Array([0b10101100, 0b10111101]));
      dataBuffer.writeBit(false);

      expect(byteArray[0]).toBe(0b11111111);
      expect(byteArray[1]).toBe(0b11010110);
      expect(byteArray[2]).toBe(0b01011110);
      expect(byteArray[3]).toBe(0b10000000);
    });
  });

  describe('ToBytes', () => {

    it('should return empty array when no data is written', () => {
      const byteArray = new Uint8Array(2);
      const dataBuffer = new DataBufferWriter(byteArray);

      const bytes = dataBuffer.toBytes();

      expect(bytes.length).toBe(0);
    });

    it('should return one byte array when one bit is written', () => {
      const byteArray = new Uint8Array(2);
      const dataBuffer = new DataBufferWriter(byteArray);

      dataBuffer.writeBit(true);

      const bytes = dataBuffer.toBytes();

      expect(bytes.length).toBe(1);
    });

    it('should return one byte array when eight bits are written', () => {
      const byteArray = new Uint8Array(2);
      const dataBuffer = new DataBufferWriter(byteArray);

      for (let i = 0; i < 8; i++) {
        dataBuffer.writeBit(i % 2 === 0);
      }

      const bytes = dataBuffer.toBytes();

      expect(bytes.length).toBe(1);
    });

    it('should return two bytes array when twelve bits are written', () => {
      const byteArray = new Uint8Array(2);
      const dataBuffer = new DataBufferWriter(byteArray);

      for (let i = 0; i < 12; i++) {
        dataBuffer.writeBit(i % 2 === 0);
      }

      const bytes = dataBuffer.toBytes();

      expect(bytes.length).toBe(2);
    });

    it('should return two bytes array when sixteen bits are written', () => {
      const byteArray = new Uint8Array(2);
      const dataBuffer = new DataBufferWriter(byteArray);

      for (let i = 0; i < 16; i++) {
        dataBuffer.writeBit(i % 2 === 0);
      }

      const bytes = dataBuffer.toBytes();

      expect(bytes.length).toBe(2);
    });
  });

  describe('Bug: Populated Array', () => {

    it('should modify buffer when writing to a populated array with one bit followed by one byte', () => {
      let byteArray = new Uint8Array([0b11111111, 0b11111111]);
      let dataBuffer = new DataBufferWriter(byteArray);

      dataBuffer.writeBit(false);
      dataBuffer.writeByte(0b10101010);

      expect(byteArray[0]).toBe(0b01010101);
      expect(byteArray[1]).toBe(0b00000000);

      byteArray = new Uint8Array([0b11111111, 0b11111111]);
      dataBuffer = new DataBufferWriter(byteArray);

      dataBuffer.writeBit(true);
      dataBuffer.writeByte(0b01010101);

      expect(byteArray[0]).toBe(0b10101010);
      expect(byteArray[1]).toBe(0b10000000);
    });

    it('should modify buffer when writing to a populated array with two bits followed by one byte', () => {
      const byteArray = new Uint8Array([0b11111111, 0b11111111]);
      const dataBuffer = new DataBufferWriter(byteArray);

      dataBuffer.writeBit(false);
      dataBuffer.writeBit(true);
      dataBuffer.writeByte(0b01010101);

      console.log(printBits(byteArray[0]));
      console.log(printBits(byteArray[1]));

      expect(byteArray[0]).toBe(0b01010101);
      expect(byteArray[1]).toBe(0b01000000);
    });

    it('should modify buffer when writing to a populated array with three bits followed by two bytes', () => {
      const byteArray = new Uint8Array([0b11111111, 0b11111111, 0b11111111]);
      const dataBuffer = new DataBufferWriter(byteArray);

      dataBuffer.writeBit(false);
      dataBuffer.writeBit(true);
      dataBuffer.writeBit(false);

      dataBuffer.writeBytes(new Uint8Array([0b10101010, 0b10101010]));

      expect(byteArray[0]).toBe(0b01010101);
      expect(byteArray[1]).toBe(0b01010101);
      expect(byteArray[2]).toBe(0b01000000);
    });
  });
});